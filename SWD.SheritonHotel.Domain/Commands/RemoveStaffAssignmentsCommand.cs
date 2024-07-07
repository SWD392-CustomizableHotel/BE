using MediatR;

namespace SWD.SheritonHotel.Domain.Commands
{
    public class RemoveStaffAssignmentsCommand : IRequest<bool>
    {
        public int ServiceId { get; set; }
    }
}
