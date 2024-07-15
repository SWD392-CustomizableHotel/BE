using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWD.SheritonHotel.Domain.Commands;
using SWD.SheritonHotel.Domain.Queries;

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
        //[Authorize(Roles = "CUSTOMER")]
        [Route("create-payment")]
        public async Task<IActionResult> CreatePayment(CreatePaymentCommand command)
        {
            var paymentId = await _mediator.Send(command);
            return Ok(paymentId);
        }

        [HttpGet]
        [Authorize(Roles = "STAFF")]
        [Route("get-payment")]
        public async Task<IActionResult> GetPaymentId([FromQuery] GetPaymentIdQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
