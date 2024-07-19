using MediatR;
using SWD.SheritonHotel.Domain.DTO.Account;
using System.Collections.Generic;

namespace SWD.SheritonHotel.Domain.Queries.AccountQuery
{
    public class GetStaffQuery : IRequest<List<StaffDTO>>
    {
    }
}
