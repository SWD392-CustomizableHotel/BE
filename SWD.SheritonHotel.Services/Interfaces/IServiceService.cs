using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.OtherObjects;

namespace SWD.SheritonHotel.Services.Interfaces;

public interface IServiceService
{
    Task<(List<ServiceDto>, int)> GetAllServicesAsync(int pageNumber, int pageSize, ServiceFilter? serviceFilter, string searchTerm = null);
}