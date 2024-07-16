using Entities;
using Microsoft.AspNetCore.Http;
using SWD.SheritonHotel.Domain.Base;
using SWD.SheritonHotel.Domain.OtherObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Data.Repositories.Interfaces
{
    public interface IRoomRepository : IBaseRepository<Room>
    {
        Task<int> CreateRoomAsync(Room room, IFormFile imageFile);

        Task<int> GetTotalRoomsCountAsync();
        Task<(List<Room>, int)> GetRoomsAsync(int pageNumber, int pageSize,
                    RoomFilter? roomFilter, string searchTerm = null);
        Task<Room> UpdateRoomStatusAsync(int roomId, string status, string updatedBy);

        Task DeleteRoomAsync(int roomId);

        Task<Room> GetRoomByIdAsync(int roomId);

        Task<Room> UpdateRoomAsync(int roomId, string type, decimal price, IFormFile imageFile = null, string updatedBy = null);
        Task<List<Room>> GetAllQueryableWithInclude(CancellationToken cancellationToken, string? roomSize, int? numberOfPeople);
    }
}
