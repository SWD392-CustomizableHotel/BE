using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWD.SheritonHotel.Domain.Commands;
using SWD.SheritonHotel.Domain.OtherObjects;
using SWD.SheritonHotel.Domain.Queries;
using SWD.SheritonHotel.Services.Interfaces;

namespace SWD.SheritonHotel.API.Controllers
{
    public class AmenityController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IAmenityService _amenityService;
        public AmenityController(IMediator mediator, IAmenityService amenityService)
        {
            _mediator = mediator;
            _amenityService = amenityService;
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        [Route("get-amenities")]
        public async Task<IActionResult> GetAllAmenities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] AmenityFilter amenityFilter = null, [FromQuery] string searchTerm = null)
        {
            var paginationFilter = new PaginationFilter(pageNumber, pageSize);
            var query = new GetAllAmenitiesQuery(paginationFilter, amenityFilter, searchTerm);
            var rooms = await _mediator.Send(query);
            return Ok(rooms);
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        [Route("create-amenity")]
        public async Task<IActionResult> CreateAmenity(CreateAmenityCommand command)
        {
            var newAmenity = await _mediator.Send(command);
            return Ok(newAmenity);
        }


        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        [Route("get-amenity-details/{amenityId}")]
        public async Task<IActionResult> GetAmenityDetails(int amenityId)
        {
            var query = new GetAmenityByIdQuery(amenityId);
            var amenityDetails = await _mediator.Send(query);
            return Ok(amenityDetails);
        }

        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        [Route("update-amenity")]
        public async Task<IActionResult> UpdateAmenity(int amenityId, string name, string description, decimal price)
        {
            var command = new UpdateAmenityCommand
            {
                AmenityId = amenityId,
                Name = name,
                Description = description,
                Price = price
            };

            var result = await _mediator.Send(command);

            if (result.IsSucceeded)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        [Route("update-amenity-status")]
        public async Task<IActionResult> UpdateAmenityStatus(int amenityId, string status)
        {
            var command = new UpdateAmenityStatusCommand
            {
                AmenityId = amenityId,
                Status = status
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete]
        [Authorize(Roles = "ADMIN")]
        [Route("delete-amenity/{amenityId}")]
        public async Task<IActionResult> DeleteAmenity(int amenityId)
        {
            var command = new DeleteAmenityCommand
            {
                AmenityId = amenityId
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
