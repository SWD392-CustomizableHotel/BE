using Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWD.SheritonHotel.Domain.OtherObjects;
using SWD.SheritonHotel.Domain.Commands;
using SWD.SheritonHotel.Domain.Queries;
using Entities;
using SWD.SheritonHotel.Domain.DTO;

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

        [HttpPost]
        [Route("seed-hotels")]
        public async Task<IActionResult> SeedHotels()
        {
            var hotels = new List<Hotel>
            {
                new Hotel
                {
                    Code = "Hotel 1",
                    Address = "123 Main St",
                    Phone = "123-456-7890",
                    Description = "A lovely hotel in the heart of the city",
                    CreatedBy = "System",
                    IsDeleted = false,
                    CreatedDate = DateTime.Now,
                    LastUpdatedBy = "System"
                },
                new Hotel
                {
                    Code = "Hotel 2",
                    Address = "456 Elm St",
                    Phone = "987-654-3210",
                    Description = "A cozy hotel near the park",
                    CreatedBy = "System",
                    IsDeleted = false,
                    CreatedDate = DateTime.Now,
                    LastUpdatedBy = "System"
                }
            };

            var command = new SeedsHotelCommand(hotels);
            var result = await _mediator.Send(command);

            return Ok(new BaseResponse<Hotel> {
                IsSucceed = true,
                Results = hotels,
                Message = "Seeds the hotel successfully"
            });
        }
    }
}
