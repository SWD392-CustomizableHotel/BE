using SWD.SheritonHotel.Domain.Entities;

namespace SWD.SheritonHotel.Data.Repositories.Interfaces;

public interface IAssignServiceRepository
{
    Task<AssignedService> AssignServiceToStaff(AssignedService assignedService);
    
}