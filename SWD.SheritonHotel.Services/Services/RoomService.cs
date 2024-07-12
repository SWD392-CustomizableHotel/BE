
using Entities;
using Microsoft.AspNetCore.Http;
using SWD.SheritonHotel.Data.Repositories.Interfaces;
using SWD.SheritonHotel.Domain.OtherObjects;
using SWD.SheritonHotel.Services.Interfaces;

namespace SWD.SheritonHotel.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;
        public RoomService(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<int> CreateRoomAsync(Room room, IFormFile imageFile)
        {
            return await _roomRepository.CreateRoomAsync(room, imageFile);
        }

        public async Task<int> GetTotalRoomsCountAsync()
        {
            return await _roomRepository.GetTotalRoomsCountAsync();
        }

        public async Task<(List<Room>, int)> GetRoomsAsync(int pageNumber, int pageSize,
                    RoomFilter? roomFilter, string searchTerm = null)
        {
            return await _roomRepository.GetRoomsAsync(pageNumber, pageSize, roomFilter, searchTerm);
        }

        public async Task<Room> UpdateRoomStatusAsync(int roomId, string status, string updatedBy)
        {
            return await _roomRepository.UpdateRoomStatusAsync(roomId, status, updatedBy);
        }

        public async Task DeleteRoomAsync(int roomId)
        {
            await _roomRepository.DeleteRoomAsync(roomId);
        }

        public async Task<Room> GetRoomByIdAsync(int roomId)
        {
            return await _roomRepository.GetRoomByIdAsync(roomId);
        }

        public async Task<Room> UpdateRoomAsync(int roomId, string type, decimal price)
        {
            return await _roomRepository.UpdateRoomAsync(roomId, type, price);
        }

        public async Task<List<Room>> GetAllCustomizableRoomsAsync(CancellationToken cancellationToken = default, string? roomSize = null, int? numberOfPeople = 1)
        {
            var results = await _roomRepository.GetAllQueryableWithInclude(cancellationToken, roomSize, numberOfPeople);
            return results;
        }

        public Room UpdateRoom(Room room)
        {
            _roomRepository.Update(room);
            _roomRepository.SaveChanges();
            return room;
        }
    }
}