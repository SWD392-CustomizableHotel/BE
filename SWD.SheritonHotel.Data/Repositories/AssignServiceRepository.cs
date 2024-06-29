using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore;
using SWD.SheritonHotel.Data.Base;
using SWD.SheritonHotel.Data.Context;
using SWD.SheritonHotel.Data.Repositories.Interfaces;
using SWD.SheritonHotel.Domain.DTO;
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
        var serviceExists = await _context.Service.AnyAsync(s => s.Id == assignedService.ServiceId);
        var userExists = await _context.Users.AnyAsync(u => u.Id == assignedService.UserId);
        if (!serviceExists)
        {
            throw new KeyNotFoundException($"Service with ID {assignedService.ServiceId} not found.");
        }
        _context.AssignedServices.Add(assignedService);
        await _context.SaveChangesAsync();
        return assignedService;
    }
}