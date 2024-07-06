using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWD.SheritonHotel.Domain.Commands;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.OtherObjects;
using SWD.SheritonHotel.Domain.Queries;

namespace SWD.SheritonHotel.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class BookingController : ControllerBase
{
    private readonly IMediator _mediator;
    public BookingController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("history")]
    public async Task<IActionResult> GetBookingHistory([FromQuery] BookingFilter bookingFilter,
        [FromQuery] string? searchTerm = null, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Lấy user id từ claims
            if (userId == null)
            {
                return Unauthorized();
            }
            var paginationFilter = new PaginationFilter(pageNumber, pageSize);
            var query = new GetBookingHistoryQuery(userId, paginationFilter, bookingFilter, searchTerm);
            var response = await _mediator.Send(query);
            return Ok(response);
        }
        catch (Exception e)
        {
            return Ok(new BaseResponse<BookingHistoryDto>
            {
                IsSucceed = false,
                Result = null,
                Message = "Booking history not found!"
            });
        }
    }
    [HttpPost]
    [Authorize(Roles = "CUSTOMER")]
    [Route("create-booking")]
    public async Task<IActionResult> CreateBooking(CreateBookingCommand command)
    {
        var bookingId = await _mediator.Send(command);
        return Ok(bookingId);
    }
}