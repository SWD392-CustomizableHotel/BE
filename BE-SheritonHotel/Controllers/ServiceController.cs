using System.Linq.Dynamic.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.OtherObjects;
using SWD.SheritonHotel.Domain.Queries;

namespace SWD.SheritonHotel.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ServiceController : ControllerBase
{
    private readonly IMediator _mediator;

    public ServiceController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllServices([FromQuery] ServiceFilter serviceFilter, [FromQuery] string? searchTerm = null, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var paginationFilter = new PaginationFilter(pageNumber, pageSize);
            var query = new GetAllServicesQuery(paginationFilter, serviceFilter, searchTerm);
            var response = await _mediator.Send(query);
            return Ok(response);
        }
        catch (Exception e)
        {
            return Ok(new BaseResponse<ServiceDto> { IsSucceed = false, Result = null, Message = "Service Not Found!" });
        }
    }
}