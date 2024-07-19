using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWD.SheritonHotel.Domain.Commands;
using SWD.SheritonHotel.Domain.Commands.ServiceCommand;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.DTO.Responses;
using SWD.SheritonHotel.Domain.DTO.Service;
using SWD.SheritonHotel.Domain.OtherObjects;
using SWD.SheritonHotel.Domain.Queries;
using SWD.SheritonHotel.Domain.Queries.ServiceQuery;

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
    [HttpPost]
    public async Task<IActionResult> AssignServiceToStaff([FromBody] AssignServiceDto? assignServiceDto)
    {
        try
        {
            var command = new AssignServiceCommand(assignServiceDto);
            var response = await _mediator.Send(command);
            return Ok(response);
        }
        catch (Exception e)
        {
            return Ok(new BaseResponse<AssignServiceDto> { IsSucceed = false, Result = null, Message = "Assign Service Failed!" });
        }
    }
    [HttpGet]
    [Authorize(Roles = StaticUserRoles.ADMIN)]
    public async Task<IActionResult> GetAllAssignServices([FromQuery] AssignServiceFilter serviceFilter, [FromQuery] string? searchTerm = null, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var paginationFilter = new PaginationFilter(pageNumber, pageSize);
            var query = new GetAllAssignServicesQuery(paginationFilter, serviceFilter, searchTerm);
            var response = await _mediator.Send(query);
            return Ok(response);
        }
        catch (Exception e)
        {
            return Ok(new BaseResponse<ServiceListDto> { IsSucceed = false, Result = null, Message = "Service Not Found!" });
        }
    }
    
}