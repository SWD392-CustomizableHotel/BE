using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.Entities;
using SWD.SheritonHotel.Domain.OtherObjects;

namespace SWD.SheritonHotel.Services.Interfaces;

public interface IAssignServiceService
{
    Task<AssignedService> AssignServiceToStaffAsync(AssignServiceDto assignServiceDto);
    Task<(List<ServiceListDto>, int)> GetAllServicesAsync(int pageNumber, int pageSize, AssignServiceFilter? assignServiceFilter, string searchTerm = null);
}