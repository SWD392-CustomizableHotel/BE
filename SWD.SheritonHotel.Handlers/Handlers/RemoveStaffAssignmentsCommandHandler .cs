using MediatR;
using SWD.SheritonHotel.Data.Repositories.Interfaces;
using SWD.SheritonHotel.Domain.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Handlers.Handlers
{
    public class RemoveStaffAssignmentsCommandHandler : IRequestHandler<RemoveStaffAssignmentsCommand, bool>
    {
        private readonly IServiceRepository _serviceRepository;

        public RemoveStaffAssignmentsCommandHandler(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        public async Task<bool> Handle(RemoveStaffAssignmentsCommand request, CancellationToken cancellationToken)
        {
            var service = await _serviceRepository.GetServiceByIdAsync(request.ServiceId);

            if (service == null)
            {
                return false; // Service not found
            }

            service.AssignedStaff.Clear();

            await _serviceRepository.UpdateAsync(service);

            // Explicitly remove entries from the join table
            _serviceRepository.RemoveStaffAssignments(request.ServiceId);

            return true;
        }
    }
}
