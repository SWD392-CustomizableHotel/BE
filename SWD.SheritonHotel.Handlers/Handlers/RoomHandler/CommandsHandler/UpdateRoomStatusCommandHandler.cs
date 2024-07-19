using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SWD.SheritonHotel.Data.Repositories.Interfaces;
using SWD.SheritonHotel.Services.Interfaces;
using SWD.SheritonHotel.Domain.DTO.Responses;
using SWD.SheritonHotel.Domain.Entities;
using SWD.SheritonHotel.Domain.Commands.RoomCommand;

namespace SWD.SheritonHotel.Handlers.Handlers.RoomHandler.CommandsHandler
{
    public class UpdateRoomStatusCommandHandler : IRequestHandler<UpdateRoomStatusCommand, ResponseDto<Room>>
    {
        private readonly IRoomService _roomService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UpdateRoomStatusCommandHandler(IRoomService roomService, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _roomService = roomService;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseDto<Room>> Handle(UpdateRoomStatusCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

            try
            {
                var updatedRoom = await _roomService.UpdateRoomStatusAsync(request.RoomId,
                    request.RoomStatus, user.UserName);
                return new ResponseDto<Room>(updatedRoom);
            }
            catch (Exception ex)
            {
                return new ResponseDto<Room>
                {
                    IsSucceeded = false,
                    Message = "An error occurred while updating the room status.",
                    Errors = new[] { ex.Message }
                };
            }
        }
    }
}
