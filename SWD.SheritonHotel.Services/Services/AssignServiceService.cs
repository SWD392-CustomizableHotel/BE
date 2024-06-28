using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using SWD.SheritonHotel.Data.Repositories.Interfaces;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.Entities;
using SWD.SheritonHotel.Services.Interfaces;

namespace SWD.SheritonHotel.Services.Services;

public class AssignServiceService : IAssignServiceService
{
    private readonly IAssignServiceRepository _assignServiceRepository;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public AssignServiceService(IAssignServiceRepository assignServiceRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _assignServiceRepository = assignServiceRepository;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<AssignedService> AssignServiceToStaffAsync(AssignServiceDto assignServiceDto)
    {
        var assignedService = _mapper.Map<AssignedService>(assignServiceDto);
        var currentUser = _httpContextAccessor.HttpContext.User;
        assignedService.CreatedBy = currentUser?.FindFirstValue(ClaimTypes.NameIdentifier) ?? "system";
        assignedService.CreatedDate = DateTime.UtcNow;
        assignedService.LastUpdatedBy = currentUser?.FindFirstValue(ClaimTypes.NameIdentifier) ?? "system";
        assignedService.LastUpdatedDate = assignedService.CreatedDate;
        return await _assignServiceRepository.AssignServiceToStaff(assignedService);
    }
}