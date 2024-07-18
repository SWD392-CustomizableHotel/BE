using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SWD.SheritonHotel.Data.Base;
using SWD.SheritonHotel.Data.Context;
using SWD.SheritonHotel.Data.Repositories.Interfaces;
using SWD.SheritonHotel.Domain.DTO.Service;
using SWD.SheritonHotel.Domain.Entities;
using SWD.SheritonHotel.Domain.OtherObjects;

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
        if (!userExists)
        {
            throw new KeyNotFoundException($"User with ID {assignedService.UserId} not found.");
        }
        _context.AssignedServices.Add(assignedService);
        await _context.SaveChangesAsync();
        return assignedService;
    }

    public async Task<(List<ServiceListDto>, int)> GetAllServicesAsync(int pageNumber, int pageSize, AssignServiceFilter serviceFilter, string searchTerm = null)
    {
        var query = _context.Service
            .Include(s => s.Hotel)
            .AsQueryable();

        // Apply filters
        if (serviceFilter != null && !string.IsNullOrEmpty(serviceFilter.ServiceName))
        {
            query = query.Where(s => s.Name.Contains(serviceFilter.ServiceName));
        }

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(s => s.Name.Contains(searchTerm) || s.Description.Contains(searchTerm));
        }

        var totalRecords = await query.CountAsync();

        var services = await query
            .OrderBy(s => s.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(s => new ServiceListDto()
            {
                Id = s.Id,
                Name = s.Name,
                Price = s.Price,
                Status = s.Status,
                Description = s.Description,
                Code = s.Code,
                HotelId = s.HotelId,
                UserName = s.AssignedServices
                    .Where(asn => asn.ServiceId == s.Id)
                    .OrderByDescending(asn => asn.AssignedServiceId)
                    .Select(asn => asn.User != null ? asn.User.UserName : null)
                    .FirstOrDefault()
            })
            .ToListAsync();

        return (services, totalRecords);
    }
}