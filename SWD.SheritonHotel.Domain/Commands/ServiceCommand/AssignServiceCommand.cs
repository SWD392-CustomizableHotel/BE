using MediatR;
using SWD.SheritonHotel.Domain.DTO.Responses;
using SWD.SheritonHotel.Domain.DTO.Service;
using SWD.SheritonHotel.Domain.Entities;

namespace SWD.SheritonHotel.Domain.Commands.ServiceCommand;

public class AssignServiceCommand : IRequest<ResponseDto<AssignedService>>
{
    public AssignServiceDto AssignServiceDto { get; }

    public AssignServiceCommand(AssignServiceDto assignServiceDto)
    {
        AssignServiceDto = assignServiceDto;
    }
}