using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWD.SheritonHotel.Domain.Commands;
using SWD.SheritonHotel.Domain.DTO;

namespace SWD.SheritonHotel.API.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "ADMIN")]
public class AssignServiceController : ControllerBase
{
    private readonly IMediator _mediator;

    public AssignServiceController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost("AssignServiceToStaff")]
    public async Task<IActionResult> AssignServiceToStaff(string userId, int serviceId)
    {
        var command = new AssignServiceCommand(userId, serviceId);
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}