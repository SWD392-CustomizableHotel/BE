using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWD.SheritonHotel.Domain.Commands;

namespace SWD.SheritonHotel.API.Controllers
{
    public class BookingController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BookingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "CUSTOMER")]
        [Route("create-booking")]
        public async Task<IActionResult> CreateBooking(CreateBookingCommand command)
        {
            var bookingId = await _mediator.Send(command);
            return Ok(bookingId);
        }
    }
}
