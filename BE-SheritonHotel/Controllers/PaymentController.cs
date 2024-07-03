using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWD.SheritonHotel.Domain.Commands;

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
        [Authorize(Roles = "CUSTOMER")]
        [Route("create-payment")]
        public async Task<IActionResult> CreatePayment(CreatePaymentCommand command)
        {
            var paymentId = await _mediator.Send(command);
            return Ok(paymentId);
        }
    }
}
