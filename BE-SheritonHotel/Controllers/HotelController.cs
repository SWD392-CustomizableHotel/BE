using Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWD.SheritonHotel.Domain.OtherObjects;
using SWD.SheritonHotel.Domain.Commands;
using SWD.SheritonHotel.Domain.Queries;
using SWD.SheritonHotel.Domain.DTO;
using Swashbuckle.AspNetCore.Annotations;
using SWD.SheritonHotel.Domain.DTO.Responses;
using SWD.SheritonHotel.Domain.Entities;
using SWD.SheritonHotel.Domain.Commands.HotelCommand;
using SWD.SheritonHotel.Domain.Queries.HotelQuery;

namespace SWD.SheritonHotel.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly IMediator _mediator;
        public HotelController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [SwaggerOperation(Summary = "Get all hotels")]
        [HttpGet]
        public async Task<IActionResult> GetAllHotels()
        {
            var query = new GetAllHotelsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [SwaggerOperation(Summary = "Seed available hotels by system", Description = "This API is just for seeding hotels")]
        [HttpPost]
        [Route("seed-hotels")]
        public async Task<IActionResult> SeedHotels()
        {
            var hotels = new List<Hotel>
            {
                new Hotel
                {
                    Code = "Ho Chi Minh",
                    Address = "Ho Chi Minh",
                    Phone = "0963500436",
                    Description = "A lovely hotel in the heart of the city",
                    CreatedBy = "System",
                    IsDeleted = false,
                    CreatedDate = DateTime.Now,
                    LastUpdatedBy = "System"
                },
                new Hotel
                {
                    Code = "Ha Noi",
                    Address = "Ha Noi",
                    Phone = "0963500436",
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
