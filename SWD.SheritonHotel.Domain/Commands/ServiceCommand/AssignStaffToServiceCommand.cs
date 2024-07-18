using MediatR;
using System.Collections.Generic;

namespace SWD.SheritonHotel.Domain.Commands.ServiceCommand
{
    public class AssignStaffToServiceCommand : IRequest<bool>
    {
        public int ServiceId { get; set; }
        public List<string> StaffIds { get; set; }

        public AssignStaffToServiceCommand(int serviceId, List<string> staffIds)
        {
            ServiceId = serviceId;
            StaffIds = staffIds;
        }
    }
}
