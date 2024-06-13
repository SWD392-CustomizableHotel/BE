using Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWD.SheritonHotel.Domain.OtherObjects;
using SWD.SheritonHotel.Domain.Commands;
using SWD.SheritonHotel.Domain.Queries;

namespace SWD.SheritonHotel.API.Controllers
{
    public class RoomController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RoomController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        [Route("create-room")]
        public async Task<IActionResult> CreateRoom(CreateRoomCommand command)
        {
            var roomId = await _mediator.Send(command);
            return Ok(roomId);
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        [Route("get-rooms")]
        public async Task<IActionResult> GetAllRooms([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] RoomFilter roomFilter = null, [FromQuery] string searchTerm = null)
        {
            var paginationFilter = new PaginationFilter(pageNumber, pageSize);
            var query = new GetAllRoomsQuery(paginationFilter, roomFilter, searchTerm);
            var rooms = await _mediator.Send(query);
            return Ok(rooms);
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        [Route("get-room-details/{roomId}")]
        public async Task<IActionResult> GetRoomDetails(int roomId)
        {
            var query = new GetRoomDetailsQuery(roomId);
            var roomDetails = await _mediator.Send(query);
            return Ok(roomDetails);
        }

        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        [Route("update-room-status")]
        public async Task<IActionResult> UpdateRoomStatus(int roomId, string status)
        {
            var command = new UpdateRoomStatusCommand
            {
                RoomId = roomId,
                RoomStatus = status
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete]
        [Authorize(Roles = "ADMIN")]
        [Route("delete-room/{roomId}")]
        public async Task<IActionResult> DeleteRoom(int roomId)
        {
            var command = new DeleteRoomCommand
            {
                RoomId = roomId
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        [Route("edit-room-details/{roomId}")]
        public async Task<IActionResult> EditRoomDetails(int roomId, string type, decimal price)
        {
            var command = new EditRoomDetailsCommand(roomId, type, price);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
