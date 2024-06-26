using Entities;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.OtherObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Services.Interfaces
{
    public interface IAmenityService
    {
        Task<Amenity> CreateAmenityAsync(Amenity amenity);
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
    }
}
