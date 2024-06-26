using Entities;
using Microsoft.AspNetCore.Identity;
using SWD.SheritonHotel.Data.Repositories.Interfaces;
using SWD.SheritonHotel.Services.Interfaces;

namespace SWD.SheritonHotel.Services.Services;

public class AdminService : IAdminService
{
    private readonly IUserRepository _userRepository;
    private readonly IServiceRepository _serviceRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    
    public AdminService(
        IUserRepository userRepository,
        IServiceRepository serviceRepository,
        UserManager<ApplicationUser> userManager)
    {
        _userRepository = userRepository;
        _serviceRepository = serviceRepository;
        _userManager = userManager;
    }
    
    public async Task AssignStaffToServiceAsync(string staffId, int serviceId)
    {
        var staff = await _userRepository.GetUserByIdAsync(staffId);
        if (staff == null)
        {
            throw new KeyNotFoundException($"User with ID {staffId} not found.");
        }

        // Check if the user has the role "STAFF"
        var userRoles = await _userManager.GetRolesAsync(staff);
        if (!userRoles.Contains("STAFF"))
        {
            throw new InvalidOperationException("User must have role 'STAFF' to be assigned to a service.");
        }

        var service = await _serviceRepository.GetServiceByIdAsync(serviceId);
        if (service == null)
        {
            throw new KeyNotFoundException($"Service with ID {serviceId} not found.");
        }

        
        service.StaffId = staffId;
        await _serviceRepository.UpdateServiceAsync(service);
    }
}