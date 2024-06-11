using DbContext;
using Entities;
using Microsoft.EntityFrameworkCore;
using SWD.SheritonHotel.Data.Repositories.Interfaces;
using SWD.SheritonHotel.Domain.OtherObjects;

namespace SWD.SheritonHotel.Data.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly ApplicationDbContext _context;

        public RoomRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateRoomAsync(Room room)
        {
            _context.Room.Add(room);
            await _context.SaveChangesAsync();
            return room.Id;
        }

        public async Task<int> GetTotalRoomsCountAsync()
        {
            return await _context.Room.CountAsync();
        }
        public async Task<List<Room>> GetRoomsAsync(int pageNumber, int pageSize, RoomFilter? roomFilter)
        {
            var rooms = _context.Room.AsQueryable().AsNoTracking();

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

            return await rooms.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
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
            var room = await _context.Room.FindAsync(roomId);
            if (room != null)
            {
                return room;
            }
            else
            {
                throw new KeyNotFoundException($"No room found with ID {roomId}");
            }
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
    }
}
