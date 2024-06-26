using AutoMapper;
using SWD.SheritonHotel.Data.Repositories.Interfaces;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.Entities;
using SWD.SheritonHotel.Services.Interfaces;

namespace SWD.SheritonHotel.Services.Services;

public class AssignServiceService : IAssignServiceService
{
    private readonly IAssignServiceRepository _assignServiceRepository;
    private readonly IMapper _mapper;

    public AssignServiceService(IAssignServiceRepository assignServiceRepository, IMapper mapper)
    {
        _assignServiceRepository = assignServiceRepository;
        _mapper = mapper;
    }

    public async Task<AssignedService> AssignServiceToStaff(string userId, int serviceId)
    {
        var assignedService = new AssignedService
        {
            UserId = userId,
            ServiceId = serviceId
        };

        return await _assignServiceRepository.AssignServiceToStaff(assignedService);
    }
}