using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Swashbuckle.AspNetCore.Annotations;
using SWD.SheritonHotel.Domain.Commands.AmenityCommand;
using SWD.SheritonHotel.Domain.DTO.Responses;
using SWD.SheritonHotel.Domain.Entities;
using SWD.SheritonHotel.Domain.OtherObjects;
using SWD.SheritonHotel.Domain.Queries.AmenityQuery;
using SWD.SheritonHotel.Services.Interfaces;

namespace SWD.SheritonHotel.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        [SwaggerOperation(Summary = "Get all amenities for paging number, size, filter, search term")]
        public async Task<IActionResult> GetAllAmenities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] AmenityFilter amenityFilter = null, [FromQuery] string? searchTerm = null)
        {
            var paginationFilter = new PaginationFilter(pageNumber, pageSize);
            var query = new GetAllAmenitiesQuery(paginationFilter, amenityFilter, searchTerm);
            var rooms = await _mediator.Send(query);
            return Ok(rooms);
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        [Route("amenity/{amenityId}")]
        public async Task<IActionResult> GetAmenityDetails(int amenityId)
        {
            var query = new GetAmenityByIdQuery(amenityId);
            var amenityDetails = await _mediator.Send(query);
            return Ok(amenityDetails);
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        [Route("room/{roomId}")]
        public async Task<IActionResult> GetAmenitiesByRoomId(int roomId)
        {
            var query = new GetAmenitiesByRoomIdQuery(roomId);
            var list = await _mediator.Send(query);
            return Ok(list);
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> CreateAmenity(CreateAmenityCommand command)
        {
            var newAmenity = await _mediator.Send(command);
            return Ok(newAmenity);
        }

        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> UpdateAmenity(int amenityId, string name, string description, decimal price, int capacity, int inUse)
        {
            var command = new UpdateAmenityCommand
            {
                AmenityId = amenityId,
                Name = name,
                Description = description,
                Capacity = capacity,
                InUse = inUse,
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
        [Route("status")]
        public async Task<IActionResult> UpdateAmenityStatus(int amenityId, AmenityStatus status)
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
        public async Task<IActionResult> DeleteAmenity(int amenityId)
        {
            var command = new DeleteAmenityCommand
            {
                AmenityId = amenityId
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /*
         * Lấy amenities dựa trên Amenities Type (Basic, Advanced, Family)
         */
        [HttpGet]
        [Route("type/{type}")]
        public async Task<IActionResult> GetAmenityByType(string type)
        {
            var query = new GetAmenityByTypeQuery()
            {
                AmenityType = type
            };
            var results = await _mediator.Send(query);
            return Ok(new BaseResponse<Amenity>
            {
                IsSucceed = true,
                Results = results,
                Message = "Retrieved amenity successfully"
            });
        }
    }
}
