using MediatR;
using SWD.SheritonHotel.Domain.Commands;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.Entities;
using SWD.SheritonHotel.Services.Interfaces;

namespace SWD.SheritonHotel.Handlers.Handlers;

public class AssignServiceCommandHandler : IRequestHandler<AssignServiceCommand, ResponseDto<AssignedService>>
{
    private readonly IAssignServiceService _assignServiceService;
    public AssignServiceCommandHandler(IAssignServiceService assignServiceService)
    {
        _assignServiceService = assignServiceService;
    }

    public async Task<ResponseDto<AssignedService>> Handle(AssignServiceCommand request, CancellationToken cancellationToken)
    {
        var assignedService = await _assignServiceService.AssignServiceToStaffAsync(request.AssignServiceDto);
        return new ResponseDto<AssignedService>
        {
            Data = assignedService,
            IsSucceeded = true,
            Message = "Service assigned to staff successfully"
        };
    }
}