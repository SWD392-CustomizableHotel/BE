using Entities;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.OtherObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Data.Repositories.Interfaces
{
    public interface IServiceRepository
    {
        Task<Service> GetServiceByIdAsync(int serviceId);
        Task<(List<ServiceDto>, int)> GetAllServiceAsync(int pageNumber, int pageSize, ServiceFilter serviceFilter, string searchTerm);
        Task<Service> CreateServiceAsync(Service service);
        Task<Service> UpdateServiceAsync(int serviceId,
            string name,
            string description,
            decimal price,
            DateTime startDate,
            DateTime endDate,
            string updatedBy);
        Task<Service> UpdateServiceStatus(int serviceId, string status, string updatedBy);
        Task DeleteServiceAsync(int serviceId);
        Task<List<Service>> GetServicesByRoomIdAsync(int roomId);
        Task<bool> AssignStaffToService(int serviceId, List<string> staffIds);
        Task UpdateAsync(Service service);
        void RemoveStaffAssignments(int serviceId);
    }
}
