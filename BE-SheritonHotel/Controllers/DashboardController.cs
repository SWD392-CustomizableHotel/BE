using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWD.SheritonHotel.Application.Queries;
using SWD.SheritonHotel.Domain.DTO;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "ADMIN")]
    public class DashboardController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DashboardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<DashboardDto>> GetDashboardData()
        {
            var result = await _mediator.Send(new GetDashboardDataQuery());
            return Ok(result);
        }
    }
}