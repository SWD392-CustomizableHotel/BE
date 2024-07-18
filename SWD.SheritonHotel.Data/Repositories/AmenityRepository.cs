using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SWD.SheritonHotel.Data.Base;
using SWD.SheritonHotel.Data.Context;
using SWD.SheritonHotel.Data.Repositories.Interfaces;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.Entities;
using SWD.SheritonHotel.Domain.OtherObjects;

namespace SWD.SheritonHotel.Data.Repositories
{
    public class AmenityRepository : BaseRepository<Amenity>, IAmentiyRepository
    {
        private readonly ApplicationDbContext _context;
        protected new readonly IMapper _mapper;
        public AmenityRepository(ApplicationDbContext context, IMapper mapper)
           : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Amenity> CreateAmentiyAsync(Amenity amenity)
        {
            Add(amenity);
            await _context.SaveChangesAsync();
            return amenity;
        }

        public async Task<Amenity> GetAmenityByIdAsync(int amenityId)
        {
            var amenity = await GetById(amenityId);
            return amenity ?? throw new KeyNotFoundException($"No amenity found with ID {amenityId}");
        }

        public async Task<Amenity> UpdateAmenityAsync(int amenityId, string name, string description, decimal price, int capacity, int inUse, string updatedBy)
        {
            var amenity = await GetById(amenityId);
            if (amenity != null)
            {
                amenity.Name = name;
                amenity.Description = description;
                amenity.Price = price;
                amenity.Capacity = capacity;
                amenity.InUse = inUse;
                amenity.LastUpdatedBy = updatedBy;
                Update(amenity);
                await _context.SaveChangesAsync();
                return amenity;
            }
            else
            {
                throw new KeyNotFoundException($"No amenity found with ID {amenityId}");
            }
        }

        public async Task<Amenity> UpdateAmenityStatus(int amenityId, string status, string updatedBy)
        {
            var amenity = await GetById(amenityId);
            if (amenity != null)
            {
                if (Enum.TryParse(status, true, out AmenityStatus parsedStatus))
                {
                    amenity.Status = parsedStatus;
                    amenity.LastUpdatedBy = updatedBy;
                    Update(amenity);
                    await _context.SaveChangesAsync();
                    return amenity;
                }
                else
                {
                    throw new ArgumentException($"Invalid status value: {status}");
                }
            }
            else
            {
                throw new KeyNotFoundException($"No amenity found with ID {amenityId}");
            }
        }


        public async Task DeleteAmenityAsync(int amenityId)
        {
            var amenity = await GetById(amenityId);
            if (amenity != null)
            {
                Delete(amenity);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException($"No amenity found with ID {amenityId}");
            }
        }

        public async Task<(List<Amenity>, int)> GetAllAmenityAsync(int pageNumber, int pageSize,
                    AmenityFilter? amenityFilter, string searchTerm = null)
        {
            IQueryable<Amenity> query = DbSet.AsQueryable().AsNoTracking();

            query = query.Where(a => !a.IsDeleted);

            //Filter
            if (amenityFilter != null)
            {
                if (amenityFilter.AmenityStatus.HasValue)
                {
                    query = query.Where(r => r.Status == amenityFilter.AmenityStatus.Value);
                }

                if (amenityFilter.HotelId != 0)
                {
                    query = query.Where(r => r.HotelId == amenityFilter.HotelId);
                }              
            }
            //Search
            if (!string.IsNullOrEmpty(searchTerm))
            {
                AmenityStatus? statusSearch = null;
                if (Enum.TryParse<AmenityStatus>(searchTerm, true, out var parsedStatus))
                {
                    statusSearch = parsedStatus;
                }

                query = query.Where(r => r.Code.Contains(searchTerm) ||
                                          r.Description.Contains(searchTerm) ||
                                          r.Name.Contains(searchTerm) ||
                                          (statusSearch.HasValue && r.Status == statusSearch.Value));
            }

            var totalItems = await query.Where(a => a.IsDeleted == false).CountAsync();
            var paginatedAmenties = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return (paginatedAmenties, totalItems);
        }

        public async Task<List<Amenity>> GetAmenitiesByRoomIdAsync(int roomId)
        {
            var room  = await GetQueryable<Room>().Include(r => r.Hotel).FirstOrDefaultAsync(r => r.Id  == roomId);
            if (room == null)
            {
                throw new KeyNotFoundException($"No room found with ID {roomId}");
            }

            var amenities = await GetQueryable<Amenity>()
                            .Where(a => a.HotelId == room.HotelId && !a.IsDeleted)
                            .ToListAsync();
            return amenities;
        }

        public async Task<List<Amenity>> GetAmenitiesByTypeAsync(string type)
        {
            var amenities = await GetQueryable<Amenity>()
                            .Where(entity => entity.AmenityType.ToLower().Equals(type.ToLower()))
                            .ToListAsync();
            return amenities;
        }

        public async Task Update(Amenity amenity)
        {
             _context.Update(amenity);
             await _context.SaveChangesAsync();
        }
    }
}
