using Entities;
using SWD.SheritonHotel.Domain.Base;
using SWD.SheritonHotel.Domain.OtherObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Data.Repositories.Interfaces
{
    public interface IServiceRepository : IBaseRepository<Service>
    {
        Task<Service> GetServiceByIdAsync(int serviceId);
        Task<(List<Service>, int)> GetAllServiceAsync(int pageNumber, int pageSize,
                    ServiceFilter? serviceFilter, string searchTerm = null);
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
    }
}
