using static Microsoft.EntityFrameworkCore.DbContext;
using Entities;
using Microsoft.EntityFrameworkCore;
using SWD.SheritonHotel.Data.Repositories.Interfaces;
using SWD.SheritonHotel.Domain.OtherObjects;
using SWD.SheritonHotel.Data.Context;
using SWD.SheritonHotel.Data.Base;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using FirebaseAdmin;
using Google.Cloud.Storage.V1;
using SWD.SheritonHotel.Domain.Configs.Firebase;
using Microsoft.Extensions.Options;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Storage.v1.Data;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace SWD.SheritonHotel.Data.Repositories
{
    public class RoomRepository : BaseRepository<Room>, IRoomRepository
    {
        private readonly ApplicationDbContext _context;
        protected new readonly IMapper _mapper;
        private readonly StorageClient _storageClient;
        private readonly string _bucketName;
        private const long MaxFileSizeInBytes = 8 * 1024 * 1024;
        private const int MaxImageWidth = 800; 
        private const int MaxImageHeight = 800; 

        public RoomRepository(ApplicationDbContext context, IMapper mapper, IOptions<FirebaseConfig> firebaseConfigOptions) : base(context)
        {
            _context = context;
            _mapper = mapper;

            var firebaseConfig = firebaseConfigOptions.Value;

            if (FirebaseApp.DefaultInstance == null)
            {
                FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile(firebaseConfig.KeyFilePath)
                });
            }

            _storageClient = StorageClient.Create();
            _bucketName = firebaseConfig.StorageBucket;
        }

        public async Task<int> CreateRoomAsync(Room room, IFormFile imageFile)
        {
            if (imageFile != null)
            {
                if (imageFile.Length > MaxFileSizeInBytes)
                {
                    throw new InvalidOperationException("The file size exceeds the 8 MB limit.");
                }

                var imagePath = await SaveImageAsync(imageFile);
                room.ImagePath = imagePath;
            }

            Add(room);
            await _context.SaveChangesAsync();
            return room.Id;
        }

        private async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
            var extension = Path.GetExtension(imageFile.FileName).ToLower();

            using (var memoryStream = new MemoryStream())
            {
                await imageFile.CopyToAsync(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);

                using (var image = await Image.LoadAsync(memoryStream))
                {
                    image.Mutate(x => x.Resize(new ResizeOptions
                    {
                        Mode = ResizeMode.Max,
                        Size = new SixLabors.ImageSharp.Size(MaxImageWidth, MaxImageHeight)
                    }));

                    memoryStream.SetLength(0);
                    switch (extension)
                    {
                        case ".jpg":
                        case ".jpeg":
                            await image.SaveAsJpegAsync(memoryStream);
                            break;
                        case ".png":
                            await image.SaveAsPngAsync(memoryStream);
                            break;
                        default:
                            throw new InvalidOperationException("Unsupported image format");
                    }

                    memoryStream.Seek(0, SeekOrigin.Begin);
                }

                var storageObject = await _storageClient.UploadObjectAsync(
                    bucket: _bucketName,
                    objectName: uniqueFileName,
                    contentType: imageFile.ContentType,
                    source: memoryStream
                );

                storageObject.Acl = new List<ObjectAccessControl>
                {
                    new ObjectAccessControl
                    {
                        Entity = "allUsers",
                        Role = "READER"
                    }
                };

                await _storageClient.UpdateObjectAsync(storageObject);

                var publicUrl = $"https://storage.googleapis.com/{_bucketName}/{uniqueFileName}";
                return publicUrl;
            }
        }

        public async Task<int> GetTotalRoomsCountAsync()
        {
            return await _context.Room.Where(r => !r.IsDeleted).CountAsync();
        }
        public async Task<(List<Room>, int)> GetRoomsAsync(int pageNumber, int pageSize, 
                    RoomFilter? roomFilter, string searchTerm = null)
        {
            var rooms = _context.Room.AsQueryable().AsNoTracking();
            //Filter room that is deleted
            rooms = rooms.Where(r => !r.IsDeleted);
            //Filter
            if (roomFilter != null)
            {
                if (!string.IsNullOrEmpty(roomFilter.RoomStatus))
                {
                    rooms = rooms.Where(r => r.Status == roomFilter.RoomStatus);
                }

                if (!string.IsNullOrEmpty(roomFilter.RoomType))
                {
                    rooms = rooms.Where(r => r.Type == roomFilter.RoomType);
                }
            }
            //Search
            if (!string.IsNullOrEmpty(searchTerm))
            {
                rooms = rooms.Where(r => r.Code.Contains(searchTerm) ||
                                         r.Type.Contains(searchTerm) ||
                                         r.Status.Contains(searchTerm));
            }
            // Order by CreatedDate descending
            rooms = rooms.OrderByDescending(r => r.CreatedDate);
            // Count total records that match the search term for pagination
            var totalRecords = await rooms.CountAsync();
            // Apply pagination
            var paginatedRooms = await rooms.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return (paginatedRooms, totalRecords);
        }

        public async Task<Room> UpdateRoomStatusAsync(int roomId, string status, string updatedBy)
        {
            var room = await _context.Room.FindAsync(roomId);
            if (room != null)
            {
                room.Status = status;
                room.LastUpdatedBy = updatedBy;
                await _context.SaveChangesAsync();
                return room;
            }
            else
            {
                throw new KeyNotFoundException($"No room found with ID {roomId}");
            }
        }

        public async Task DeleteRoomAsync(int roomId)
        {
            var room = await _context.Room.FindAsync(roomId);
            if (room != null)
            {
                if (room.Status == "Occupied")
                {
                    throw new InvalidOperationException("Cannot delete a room that is currently occupied.");
                }

                room.IsDeleted = true;
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException($"No room found with ID {roomId}");
            }
        }

        public async Task<Room> GetRoomByIdAsync(int roomId)
        {
            //var room = await _context.Room.FindAsync(roomId);
            var room = await _context.Room.Include(r => r.Bookings).Include(r => r.Hotel)
                .FirstOrDefaultAsync(r => r.Id == roomId && !r.IsDeleted);
            return room ?? throw new KeyNotFoundException($"No room found with ID {roomId}");
        }

        public async Task<Room> UpdateRoomAsync(int roomId, string type, decimal price)
        {
            var room = await _context.Room.FindAsync(roomId);
            if (room != null)
            {
                room.Type = type;
                room.Price = price;
                await _context.SaveChangesAsync();
                return room;
            }
            else
            {
                throw new KeyNotFoundException($"No room found with ID {roomId}");
            }
        }

        public async Task<List<Room>> GetAllQueryableWithInclude(CancellationToken cancellationToken, string? roomSize, int? numberOfPeople)
        {
            var queryable = base.GetQueryable();
            var results = await queryable
                .Where(entity => !entity.IsDeleted && 
                                entity.RoomSize.ToLower().Equals(roomSize.ToLower()) &&
                                entity.Type.ToLower().Equals("customizable") &&
                                entity.NumberOfPeople >= numberOfPeople &&
                                entity.Status == "Available")
                .Include(entity => entity.Hotel)
                .ToListAsync();
            return results;
        }
    }
}
