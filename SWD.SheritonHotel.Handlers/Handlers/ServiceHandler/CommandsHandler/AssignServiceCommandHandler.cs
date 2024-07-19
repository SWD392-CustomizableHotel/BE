using MediatR;
using SWD.SheritonHotel.Domain.Commands.ServiceCommand;
using SWD.SheritonHotel.Domain.DTO.Responses;
using SWD.SheritonHotel.Domain.Entities;
using SWD.SheritonHotel.Services.Interfaces;

namespace SWD.SheritonHotel.Handlers.Handlers.ServiceHandler.CommandsHandler;

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