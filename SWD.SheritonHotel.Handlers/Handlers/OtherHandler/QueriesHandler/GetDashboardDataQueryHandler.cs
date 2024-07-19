using MediatR;
using System.Threading;
using System.Threading.Tasks;
using SWD.SheritonHotel.Services.Interfaces;
using SWD.SheritonHotel.Domain.DTO.Responses;
using SWD.SheritonHotel.Domain.Queries.OtherQuery;

namespace SWD.SheritonHotel.Handlers.Handlers.OtherHandler.QueriesHandler
{
    public class GetDashboardDataQueryHandler : IRequestHandler<GetDashboardDataQuery, DashboardDto>
    {
        private readonly IDashboardService _dashboardService;

        public GetDashboardDataQueryHandler(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public async Task<DashboardDto> Handle(GetDashboardDataQuery request, CancellationToken cancellationToken)
        {
            return await _dashboardService.GetDashboardDataAsync();
        }
    }
}
