using MediatR;
using SWD.SheritonHotel.Data.Repositories.Interfaces;
using SWD.SheritonHotel.Domain.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Handlers.Handlers
{
    public class AssignStaffToServiceCommandHandler : IRequestHandler<AssignStaffToServiceCommand, bool>
    {
        private readonly IServiceRepository _serviceRepository;

        public AssignStaffToServiceCommandHandler(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        public async Task<bool> Handle(AssignStaffToServiceCommand request, CancellationToken cancellationToken)
        {
            var result = await _serviceRepository.AssignStaffToService(request.ServiceId, request.StaffIds);
            return result;
        }
    }
}
