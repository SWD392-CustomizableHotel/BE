using SWD.SheritonHotel.Domain.Base;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.Entities;
using SWD.SheritonHotel.Domain.OtherObjects;

namespace SWD.SheritonHotel.Data.Repositories.Interfaces;

public interface IServiceRepository : IBaseRepository<AssignedService>
{
    Task<(List<ServiceDto>, int)> GetAllServicesAsync(int pageNumber, int pageSize, ServiceFilter serviceFilter, string searchTerm = null);
}