using MediatR;
using SWD.SheritonHotel.Domain.DTO.Responses;

namespace SWD.SheritonHotel.Domain.Queries.OtherQuery
{
    public class GetDashboardDataQuery : IRequest<DashboardDto>
    {
    }
}