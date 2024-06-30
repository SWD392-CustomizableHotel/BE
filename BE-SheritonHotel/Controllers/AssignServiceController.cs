using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWD.SheritonHotel.Domain.Commands;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.Entities;

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
    public async Task<IActionResult> AssignServiceToStaff([FromBody] AssignServiceDto? assignServiceDto)
    {
        var command = new AssignServiceCommand(assignServiceDto);
        var response = await _mediator.Send(command);
        return Ok(response);
    }
}