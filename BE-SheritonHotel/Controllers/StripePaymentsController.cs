using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWD.SheritonHotel.Domain.Commands;

namespace SWD.SheritonHotel.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StripePaymentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StripePaymentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("create-payment-intent")]
        [Authorize]
        public async Task<IActionResult> CreatePaymentIntent([FromBody] CreatePaymentIntentCommand command)
        {
            var clientSecret = await _mediator.Send(command);
            return Ok(new { clientSecret });
        }

        [HttpPost]
        [Route("send-invoice-email")]
        [Authorize]
        public async Task<IActionResult> SendInvoiceEmail([FromBody] SendInvoiceCommand command)
        {
            await _mediator.Send(command);
            return Ok("Success");
        }
        [HttpPost]
        [Route("cancel-payment")]
        [Authorize]
        public async Task<IActionResult> CancelPayment([FromBody] CancelPaymentCommand command)
        {
            await _mediator.Send(command);
            return Ok("Success");
        }
    }
}
