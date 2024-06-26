using Entities;
using Microsoft.EntityFrameworkCore;
using SWD.SheritonHotel.Data.Context;
using SWD.SheritonHotel.Data.Repositories.Interfaces;

namespace SWD.SheritonHotel.Data.Repositories;

public class ServiceRepository : IServiceRepository
{
    private readonly ApplicationDbContext _context;

    public ServiceRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Service> GetServiceByIdAsync(int serviceId)
    {
        return await _context.Service.FindAsync(serviceId);
    }

    public async Task UpdateServiceAsync(Service service)
    {
        _context.Entry(service).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }
}