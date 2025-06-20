using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWD.SheritonHotel.Domain.Commands.Booking;
using SWD.SheritonHotel.Domain.Commands.PaymentCommand;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.DTO.Booking;
using SWD.SheritonHotel.Domain.DTO.Responses;
using SWD.SheritonHotel.Domain.OtherObjects;
using SWD.SheritonHotel.Domain.Queries;
using SWD.SheritonHotel.Domain.Queries.BookingQuery;

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
    [Authorize]
    public async Task<IActionResult> CreateBooking(CreateBookingCommand command)
    {
        var bookingId = await _mediator.Send(command);
        return Ok(bookingId);
    }
    
    [HttpGet]
    [Authorize(Roles = "STAFF")]
    [Route("check-out")]
    public async Task<IActionResult> GetBookingByEndDate([FromQuery] BookingFilter bookingFilter,
        [FromQuery] string? searchTerm = null, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var paginationFilter = new PaginationFilter(pageNumber, pageSize);
            var query = new GetAllBookingHistoryByEndDateQuery(paginationFilter, bookingFilter, searchTerm);
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

    [HttpGet("{id}")]
    [Authorize(Roles = "STAFF")]
    public async Task<IActionResult> ViewBookingDetails(int id)
    {
        try
        {
            var query = new GetBookingDetailsQuery(id);
            var response = await _mediator.Send(query);
            if (response == null)
            {
                return NotFound(new BaseResponse<BookingDetailsDto>
                {
                    IsSucceed = false,
                    Result = null,
                    Message = "Booking not found!"
                });
            }

            return Ok(new BaseResponse<BookingDetailsDto>
            {
                IsSucceed = true,
                Result = response,
                Message = "Booking details retrieved successfully!"
            });
        }
        catch (Exception e)
        {
            return Ok(new BaseResponse<BookingDetailsDto>
            {
                IsSucceed = false,
                Result = null,
                Message = e.Message
            });
        }
    }
    [HttpPost]
    [Authorize(Roles = "STAFF")]
    [Route("check-out")]
    public async Task<IActionResult> CheckOut([FromBody] CheckOutCommand command)
    {
        try
        {
            await _mediator.Send(command);
            return Ok(new BaseResponse<string>
            {
                IsSucceed = true,
                Result = null,
                Message = "Check Out Successful!",
            });
        }
        catch (Exception e)
        {
            return Ok(new BaseResponse<string>
            {
                IsSucceed = false,
                Result = null,
                Message = e.Message,
            });
        }
    }
    [HttpPost]
    [Authorize(Roles = "STAFF")]
    [Route("payment")]
    public async Task<IActionResult> Payment([FromBody] PaymentCommand command)
    {
        try
        {
            await _mediator.Send(command);
            return Ok(new BaseResponse<string>
            {
                IsSucceed = true,
                Result = null,
                Message = "Payment Successfull!",
            });
        }
        catch (Exception e)
        {
            return Ok(new BaseResponse<string>
            {
                IsSucceed = false,
                Result = null,
                Message = e.Message,
            });
        }
    }

	[HttpGet]
	[Authorize(Roles = "STAFF")]
	[Route("check-in")]
	public async Task<IActionResult> GetBookingByStartDate([FromQuery] CombineBookingFilter combineBookingFilter,
		[FromQuery] string? searchTerm = null, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
	{
		try
		{
			var paginationFilter = new PaginationFilter(pageNumber, pageSize);
			var query = new GetAllBookingHistoryByStartDateQuery(paginationFilter, combineBookingFilter, searchTerm);
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
}