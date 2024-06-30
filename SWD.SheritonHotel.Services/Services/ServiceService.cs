using SWD.SheritonHotel.Data.Repositories.Interfaces;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.OtherObjects;
using SWD.SheritonHotel.Services.Interfaces;

namespace SWD.SheritonHotel.Services.Services;

public class ServiceService : IServiceService
{
    private readonly IServiceRepository _serviceRepository;

    public ServiceService(IServiceRepository serviceRepository)
    {
        _serviceRepository = serviceRepository;
    }
    
    public async Task<(List<ServiceDto>, int)> GetAllServicesAsync(int pageNumber, int pageSize, ServiceFilter? serviceFilter, string searchTerm = null)
    {
        return await _serviceRepository.GetAllServicesAsync(pageNumber, pageSize, serviceFilter, searchTerm);
    }
}