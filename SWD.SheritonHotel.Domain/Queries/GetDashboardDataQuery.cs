using MediatR;
using SWD.SheritonHotel.Domain.DTO;

namespace SWD.SheritonHotel.Application.Queries
{
    public class GetDashboardDataQuery : IRequest<DashboardDto>
    {
    }
}