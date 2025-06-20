﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWD.SheritonHotel.Domain.Commands.PaymentCommand;

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
            var reponse = await _mediator.Send(command);
            return Ok(reponse);
        }

        [HttpPost]
        [Route("send-invoice-link")]
        [Authorize]
        public async Task<IActionResult> SendInvoiceLink([FromBody] SendInvoiceCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPost]
        [Route("cancel-payment")]
        [Authorize]
        public async Task<IActionResult> CancelPayment([FromBody] CancelPaymentCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }
    }
}
