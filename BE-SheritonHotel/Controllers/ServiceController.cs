using Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OtherObjects;
using SWD.SheritonHotel.Domain.Commands;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.Queries;
using SWD.SheritonHotel.Handlers.Handlers;

namespace SWD.SheritonHotel.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ServicesController : ControllerBase
	{
		private readonly IMediator _mediator;

		public ServicesController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet]
		[Route("get-all-services")]
        public async Task<IActionResult> GetServices()
        {
            var result = await _mediator.Send(new GetServicesQuery());
            return Ok(new ResponseDto<IEnumerable<Service>>(result));
        }

        [HttpGet]
		[Route("get-service-by-id/{id:int}")]
        public async Task<IActionResult> GetServiceById(int id)
        {
            var result = await _mediator.Send(new GetServiceByIdQuery { ServiceId = id });
            if (result == null)
            {
                return NotFound(new ResponseDto<Service>
                {
                    IsSucceeded = false,
                    Message = "Service not found"
                });
            }
            return Ok(new ResponseDto<Service>(result));
        }

        [HttpPost]
		[Route("create-service")]
		[Authorize(Roles = StaticUserRoles.ADMIN)]
        public async Task<IActionResult> CreateService([FromBody] CreateServiceCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.IsSucceeded)
            {
                return Ok(result);
            }
            return Unauthorized(result);
        }

        [HttpPut]
        [Route("update-service/{id:int}")]
        [Authorize(Roles = StaticUserRoles.ADMIN)]
        public async Task<IActionResult> UpdateServiceStatus(int id, [FromBody] string status)
        {
            var command = new UpdateServiceStatusCommand
            {
                ServiceId = id,
                Status = status
            };
            var result = await _mediator.Send(command);
            if (result.IsSucceeded)
            {
                return Ok(result);
            }
            return Unauthorized(result);
        }

        [HttpDelete]
		[Route("delete-service/{id:int}")]
		[Authorize(Roles = StaticUserRoles.ADMIN)]
		public async Task<IActionResult> DeleteService(int id)
		{
            var result = await _mediator.Send(new DeleteServiceCommand { ServiceId = id });
            if (result.IsSucceeded)
            {
                return Ok(result);
            }
            return Unauthorized(result);
        }
	}
}
