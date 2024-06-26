using MediatR;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.Entities;

namespace SWD.SheritonHotel.Domain.Commands;

public class AssignServiceCommand : IRequest<ResponseDto<AssignedService>>
{
    public string UserId { get; set; }
    public int ServiceId { get; set; }

    public AssignServiceCommand(string userId, int serviceId)
    {
        UserId = userId;
        ServiceId = serviceId;
    }
}