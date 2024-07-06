using Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWD.SheritonHotel.Domain.OtherObjects;
using SWD.SheritonHotel.Domain.Commands;
using SWD.SheritonHotel.Domain.Queries;

namespace SWD.SheritonHotel.API.Controllers
{
    public class HotelController : ControllerBase
    {
        private readonly IMediator _mediator;
        public HotelController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        //[Authorize(Roles = "ADMIN")]
        [Route("get-hotels")]
        public async Task<IActionResult> GetAllHotels()
        {
            var query = new GetAllHotelsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
