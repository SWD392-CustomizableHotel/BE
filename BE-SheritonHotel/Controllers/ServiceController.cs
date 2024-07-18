using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWD.SheritonHotel.Domain.Commands.ServiceCommand;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.OtherObjects;
using SWD.SheritonHotel.Domain.Queries;
using SWD.SheritonHotel.Domain.Queries.AccountQuery;
using SWD.SheritonHotel.Domain.Queries.RoomQuery;
using SWD.SheritonHotel.Domain.Queries.ServiceQuery;
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
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetAllServices([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] ServiceFilter serviceFilter = null, [FromQuery] string? searchTerm = null)
        {
            var paginationFilter = new PaginationFilter(pageNumber, pageSize);
            var query = new GetAllServicesQuery(paginationFilter, serviceFilter, searchTerm);
            var services = await _mediator.Send(query);
            return Ok(services);
        }


        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        [Route("{serviceId}")]
        public async Task<IActionResult> GetServiceById(int serviceId)
        {
            var query = new GetServiceByIdQuery(serviceId);
            var service = await _mediator.Send(query);
            return Ok(service);
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        [Route("room/{roomId}")]
        public async Task<IActionResult> GetAmenitiesByRoomId(int roomId)
        {
            var query = new GetServicesByRoomIdQuery(roomId);
            var list = await _mediator.Send(query);
            return Ok(list);
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> CreateService(CreateServiceCommand command)
        {
            var newService = await _mediator.Send(command);
            return Ok(newService);
        }

        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> UpdateService([FromBody] UpdateServiceCommand command)
        {
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
        public async Task<IActionResult> UpdateServiceStatus(int serviceId, ServiceStatus status)
        {
            var command = new UpdateServiceStatusCommand
            {
                ServiceId = serviceId,
                Status = status,
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete]
        [Authorize(Roles = "ADMIN")]
        [Route("{serviceId}")]
        public async Task<IActionResult> DeleteService(int serviceId)
        {
            var command = new DeleteServiceCommand
            {
                ServiceId = serviceId
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        [Route("assign-staff")]
        public async Task<IActionResult> AssignStaffToService(AssignStaffToServiceCommand command)
        {
            var result = await _mediator.Send(command);
            if (result)
                return Ok();
            return BadRequest();
        }
        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        [Route("get-staff")]
        public async Task<IActionResult> GetStaff()
        {
            var query = new GetStaffQuery();
            var staff = await _mediator.Send(query);
            return Ok(staff);   
        }
        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        [Route("remove-staff-assignments")]
        public async Task<IActionResult> RemoveStaffAssignments([FromBody] RemoveStaffAssignmentsCommand command)
        {
            var result = await _mediator.Send(command);
            if (!result)
            {
                return NotFound("Service not found");
            }
            return Ok("Staff assignments removed successfully");
        }

    }
}
