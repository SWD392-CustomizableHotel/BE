using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWD.SheritonHotel.Domain.Commands.IdentityCard;
using SWD.SheritonHotel.Domain.Queries;
using SWD.SheritonHotel.Domain.Queries.IdentityCardQuery;

namespace SWD.SheritonHotel.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IdentityCardController : ControllerBase
    {
        private readonly IMediator _mediator;

        public IdentityCardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "STAFF")]
        public async Task<IActionResult> UploadIdentityCard([FromForm] UploadIdentityCardCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet]
        [Authorize(Roles = "STAFF")]
        public async Task<IActionResult> GetAllIdentityCards()
        {
            var query = new GetAllIdentityCardsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
