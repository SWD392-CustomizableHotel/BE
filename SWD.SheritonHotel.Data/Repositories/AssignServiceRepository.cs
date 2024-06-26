using Microsoft.EntityFrameworkCore;
using SWD.SheritonHotel.Data.Context;
using SWD.SheritonHotel.Data.Repositories.Interfaces;
using SWD.SheritonHotel.Domain.Entities;

namespace SWD.SheritonHotel.Data.Repositories;

public class AssignServiceRepository : IAssignServiceRepository
{
    private readonly ApplicationDbContext _context;

    public AssignServiceRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AssignedService> AssignServiceToStaff(AssignedService assignedService)
    {
        _context.AssignedServices.Add(assignedService);
        await _context.SaveChangesAsync();
        return assignedService;
    }
    
}