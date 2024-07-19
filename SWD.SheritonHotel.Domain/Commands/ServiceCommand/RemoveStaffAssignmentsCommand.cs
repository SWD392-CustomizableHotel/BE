using MediatR;

namespace SWD.SheritonHotel.Domain.Commands.ServiceCommand
{
    public class RemoveStaffAssignmentsCommand : IRequest<bool>
    {
        public int ServiceId { get; set; }
    }
}
