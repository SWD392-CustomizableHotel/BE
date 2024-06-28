using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.Entities;

namespace SWD.SheritonHotel.Services.Interfaces;

public interface IAssignServiceService
{
    Task<AssignedService> AssignServiceToStaffAsync(AssignServiceDto assignServiceDto);
}