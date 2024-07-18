using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.DTO.Responses;
using SWD.SheritonHotel.Domain.Entities;
using SWD.SheritonHotel.Domain.Queries;
using SWD.SheritonHotel.Domain.Queries.RoomQuery;

namespace SWD.SheritonHotel.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/view-available-room")]
    public class ViewAvailableRoomController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ViewAvailableRoomController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoom(CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _mediator.Send(new GetAllAvailableRoomQuery(), cancellationToken);
                var response = new BaseResponse<Room>
                {
                    IsSucceed = true,
                    Results = result,
                    Message = "Rooms retrieved successfully."
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new BaseResponse<string>
                {
                    IsSucceed = false,
                    Result = null,
                    Results = null,
                    Message = $"An internal error occurred: {ex.Message}"
                };
                return StatusCode(500, response);
            }
        }
    }
}
