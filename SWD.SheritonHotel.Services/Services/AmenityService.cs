using Entities;
using SWD.SheritonHotel.Data.Repositories;
using SWD.SheritonHotel.Data.Repositories.Interfaces;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.OtherObjects;
using SWD.SheritonHotel.Services.Interfaces;

namespace SWD.SheritonHotel.Services.Services
{
    public class AmenityService : IAmenityService
    {
        private readonly IAmentiyRepository _amentiyRepository;

        public AmenityService(IAmentiyRepository amentioniyRepository)
        {
            _amentiyRepository = amentioniyRepository;
        }
        public async Task<Amenity> CreateAmenityAsync(Amenity amenity)
        {
            var newAmenity = await _amentiyRepository.CreateAmentiyAsync(amenity);
            return newAmenity;
        }

        public async Task<Amenity> GetAmenityByIdAsync(int amenityId)
        {
            return await _amentiyRepository.GetAmenityByIdAsync(amenityId);
        }

        public async Task<Amenity> UpdateAmenityAsync(int amenityId, string name, string description, decimal price, int capacity, int inUse, string updatedBy)
        {
            return await _amentiyRepository.UpdateAmenityAsync(amenityId, name, description, price, capacity, inUse, updatedBy);
        }

        public async Task<Amenity> UpdateAmenityStatus(int amenityId, string status, string updatedBy)
        {
            return await _amentiyRepository.UpdateAmenityStatus(amenityId, status, updatedBy);
        }

        public async Task DeleteAmenityAsync(int amenityId)
        {
            await _amentiyRepository.DeleteAmenityAsync(amenityId);
        }

        public async Task<(List<Amenity>, int)> GetAllAmenityAsync(int pageNumber, int pageSize,
                    AmenityFilter? amenityFilter, string searchTerm = null)
        {
            return await _amentiyRepository.GetAllAmenityAsync(pageNumber, pageSize, amenityFilter, searchTerm);
        }

        public async Task<List<Amenity>> GetAmenitiesByRoomIdAsync(int roomId)
        {
            return await _amentiyRepository.GetAmenitiesByRoomIdAsync(roomId);
        }

        public async Task<List<Amenity>> GetAmenitiesByTypeAsync(string type)
        {
            return await _amentiyRepository.GetAmenitiesByTypeAsync(type);
        }
    }
}
