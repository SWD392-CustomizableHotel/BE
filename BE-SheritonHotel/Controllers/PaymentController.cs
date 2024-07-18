using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWD.SheritonHotel.Domain.Commands.PaymentCommand;
using SWD.SheritonHotel.Domain.Queries.PaymentQuery;

namespace SWD.SheritonHotel.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PaymentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreatePayment(CreatePaymentCommand command)
        {
            var paymentId = await _mediator.Send(command);
            return Ok(paymentId);
        }

        [HttpGet]
        [Authorize(Roles = "STAFF")]
        public async Task<IActionResult> GetPaymentId([FromQuery] GetPaymentIdQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("payment-for-later")]
        [Authorize(Roles = "STAFF")]
        public async Task<IActionResult> CreatePaymentForLater(CreatePaymentForLaterCommand request)
        {
            if (request == null || request.BookingId <= 0)
            {
                return BadRequest("Invalid request");
            }

            var paymentId = await _mediator.Send(request);
            return Ok(paymentId);
        }

        [HttpPut]
        [Authorize(Roles = "STAFF")]
        public async Task<IActionResult> UpdatePaymentStatus([FromBody] UpdatePaymentStatusCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }
    }
}
