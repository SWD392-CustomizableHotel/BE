using MediatR;
using SWD.SheritonHotel.Data.Repositories.Interfaces;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.Queries;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Handlers.Handlers
{
    public class GetStaffQueryHandler : IRequestHandler<GetStaffQuery, List<StaffDTO>>
    {
        private readonly IUserRepository _userRepository;

        public GetStaffQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<StaffDTO>> Handle(GetStaffQuery request, CancellationToken cancellationToken)
        {
            var staff = await _userRepository.GetUsersByRoleAsync("STAFF");
            return staff;
        }
    }
}
