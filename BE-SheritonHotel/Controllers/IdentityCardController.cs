using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWD.SheritonHotel.Domain.Commands;

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
        [Route("upload-identity-card")]
        public async Task<IActionResult> UploadIdentityCard([FromForm] UploadIdentityCardCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
