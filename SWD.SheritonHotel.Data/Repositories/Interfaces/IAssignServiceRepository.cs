using SWD.SheritonHotel.Domain.Base;
using SWD.SheritonHotel.Domain.Entities;

namespace SWD.SheritonHotel.Data.Repositories.Interfaces;

public interface IAssignServiceRepository : IBaseRepository<AssignedService>
{
    Task<AssignedService> AssignServiceToStaff(AssignedService assignedService);
    
}