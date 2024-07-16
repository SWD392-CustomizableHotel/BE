using MediatR;
using SWD.SheritonHotel.Application.Queries;
using SWD.SheritonHotel.Domain.DTO;
using System.Threading;
using System.Threading.Tasks;
using SWD.SheritonHotel.Services.Interfaces;

namespace SWD.SheritonHotel.Application.Handlers
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
