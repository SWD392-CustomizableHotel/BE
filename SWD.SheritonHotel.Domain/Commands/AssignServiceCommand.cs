using MediatR;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.Entities;

namespace SWD.SheritonHotel.Domain.Commands;

public class AssignServiceCommand : IRequest<ResponseDto<AssignedService>>
{
    public AssignServiceDto AssignServiceDto { get; }

    public AssignServiceCommand(AssignServiceDto assignServiceDto)
    {
        AssignServiceDto = assignServiceDto;
    }
}