using Entities;
using SWD.SheritonHotel.Domain.OtherObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Services.Interfaces
{
    public interface IRoomService
    {
        Task<int> CreateRoomAsync(Room room);
        Task<int> GetTotalRoomsCountAsync();
        Task<(List<Room>, int)> GetRoomsAsync(int pageNumber, int pageSize,
                    RoomFilter? roomFilter, string searchTerm = null);
        Task<Room> UpdateRoomStatusAsync(int roomId, string status, string updatedBy);
        Task DeleteRoomAsync(int roomId);
        Task<Room> GetRoomByIdAsync(int roomId);
        Task<Room> UpdateRoomAsync(int roomId, string type, decimal price);
        Task<List<Room>> GetAllCustomizableRoomsAsync(CancellationToken cancellationToken, string? roomSize, int? numberOfPeople);
        Room UpdateRoom(Room room);
    }
}
