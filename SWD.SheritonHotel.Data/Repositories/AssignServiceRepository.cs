using Microsoft.EntityFrameworkCore;
using SWD.SheritonHotel.Data.Base;
using SWD.SheritonHotel.Data.Context;
using SWD.SheritonHotel.Data.Repositories.Interfaces;
using SWD.SheritonHotel.Domain.Entities;

namespace SWD.SheritonHotel.Data.Repositories;

public class AssignServiceRepository : BaseRepository<AssignedService>, IAssignServiceRepository
{
    private readonly ApplicationDbContext _context;

    public AssignServiceRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<AssignedService> AssignServiceToStaff(AssignedService assignedService)
    {
        base.Add(assignedService);
        await _context.SaveChangesAsync();
        return assignedService;
    }
    
}