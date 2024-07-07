using SWD.SheritonHotel.Domain.Base;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.Entities;
using SWD.SheritonHotel.Domain.OtherObjects;

namespace SWD.SheritonHotel.Data.Repositories.Interfaces;

public interface IAssignServiceRepository : IBaseRepository<AssignedService>
{
    Task<AssignedService> AssignServiceToStaff(AssignedService assignedService);
    Task<(List<ServiceListDto>, int)> GetAllServicesAsync(int pageNumber, int pageSize, AssignServiceFilter serviceFilter, string searchTerm = null);
}