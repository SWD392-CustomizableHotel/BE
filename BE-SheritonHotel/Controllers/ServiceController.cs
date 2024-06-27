using Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OtherObjects;
using SWD.SheritonHotel.Domain.Commands;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.OtherObjects;
using SWD.SheritonHotel.Domain.Queries;
using SWD.SheritonHotel.Handlers.Handlers;
using SWD.SheritonHotel.Services.Interfaces;

namespace SWD.SheritonHotel.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServicesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IManageService _amenityService;

        public ServicesController(IMediator mediator, IManageService manageService)
        {
            _mediator = mediator;
            _amenityService = manageService;
        }

        [HttpGet]
        [Authorize(Roles = StaticUserRoles.ADMIN)]
        [Route("get-all-services")]
        public async Task<IActionResult> GetAllServices([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] ServiceFilter serviceFilter = null, [FromQuery] string? searchTerm = null)
        {
            var paginationFilter = new PaginationFilter(pageNumber, pageSize);
            var query = new GetAllServicesQuery(paginationFilter, serviceFilter, searchTerm);
            var services = await _mediator.Send(query);
            return Ok(services);
        }

        [HttpGet]
        [Authorize(Roles = StaticUserRoles.ADMIN)]
        [Route("get-service-by-id/{serviceId}")]
        public async Task<IActionResult> GetServiceById(int serviceId)
        {
            var query = new GetServiceByIdQuery(serviceId);
            var service = await _mediator.Send(query);
            return Ok(service);
        }

        [HttpGet]
        [Authorize(Roles = StaticUserRoles.ADMIN)]
        [Route("get-room-amenity/{roomId}")]
        public async Task<IActionResult> GetAmenitiesByRoomId(int roomId)
        {
            var query = new GetServicesByRoomIdQuery(roomId);
            var list = await _mediator.Send(query);
            return Ok(list);
        }

        [HttpPost]
        [Authorize(Roles = StaticUserRoles.ADMIN)]
        [Route("create-service")]
        public async Task<IActionResult> CreateService(CreateServiceCommand command)
        {
            var newService = await _mediator.Send(command);
            return Ok(newService);
        }

        [HttpPut]
        [Authorize(Roles = StaticUserRoles.ADMIN)]
        [Route("update-service")]
        public async Task<IActionResult> UpdateAmenity(int serviceId, string name, string description, decimal price)
        {
            var command = new UpdateServiceCommand
            {
                ServiceId = serviceId,
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
        [Authorize(Roles = StaticUserRoles.ADMIN)]
        [Route("update-service-status")]
        public async Task<IActionResult> UpdateServiceStatus(int serviceId, ServiceStatus status)
        {
            var command = new UpdateServiceStatusCommand
            {
                ServiceId = serviceId,
                Status = status
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete]
        [Authorize(Roles = StaticUserRoles.ADMIN)]
        [Route("delete-service/{serviceId}")]
        public async Task<IActionResult> DeleteService(int serviceId)
        {
            var command = new DeleteServiceCommand
            {
                ServiceId = serviceId
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
