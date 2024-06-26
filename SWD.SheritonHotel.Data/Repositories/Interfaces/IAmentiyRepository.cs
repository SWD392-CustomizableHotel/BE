using Entities;
using SWD.SheritonHotel.Domain.Base;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.OtherObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Data.Repositories.Interfaces
{
    public interface IAmentiyRepository
    {
        Task<Amenity> CreateAmentiyAsync(Amenity amenity);
        Task<Amenity> GetAmenityByIdAsync(int amenityId);
        Task<Amenity> UpdateAmenityAsync(int amenityId,
            string name,
            string description,
            decimal price,
            string updatedBy);
        Task<Amenity> UpdateAmenityStatus(int amenityId, string status, string updatedBy);
        Task DeleteAmenityAsync(int amenityId);
        Task<(List<Amenity>, int)> GetAllAmenityAsync(int pageNumber, int pageSize,
                    AmenityFilter? amenityFilter, string searchTerm = null);
        Task<List<Amenity>> GetAmenitiesByRoomIdAsync(int roomId);
    }
}
