using MediatR;
using SWD.SheritonHotel.Domain.DTO;
using System.Collections.Generic;

namespace SWD.SheritonHotel.Domain.Queries
{
    public class GetStaffQuery : IRequest<List<StaffDTO>>
    {
    }
}
