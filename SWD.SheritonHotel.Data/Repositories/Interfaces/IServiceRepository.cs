using Entities;

namespace SWD.SheritonHotel.Data.Repositories.Interfaces;

public interface IServiceRepository
{
    Task<Service> GetServiceByIdAsync(int serviceId);
    Task UpdateServiceAsync(Service service);
}