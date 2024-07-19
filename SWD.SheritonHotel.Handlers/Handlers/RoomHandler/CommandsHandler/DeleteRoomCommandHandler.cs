using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SWD.SheritonHotel.Services.Interfaces;
using SWD.SheritonHotel.Domain.DTO.Responses;
using SWD.SheritonHotel.Domain.Entities;
using SWD.SheritonHotel.Domain.Commands.RoomCommand;

namespace SWD.SheritonHotel.Handlers.Handlers.RoomHandler.CommandsHandler
{
    public class DeleteRoomCommandHandler : IRequestHandler<DeleteRoomCommand, ResponseDto<bool>>
    {
        private readonly IRoomService _roomService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeleteRoomCommandHandler(IRoomService roomService, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _roomService = roomService;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseDto<bool>> Handle(DeleteRoomCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            if (user == null || !await _userManager.IsInRoleAsync(user, "Admin"))
            {
                return new ResponseDto<bool>
                {
                    IsSucceeded = false,
                    Message = "Unauthorized",
                    Errors = new[] { "You must be an admin to perform this operation." }
                };
            }
            try
            {
                await _roomService.DeleteRoomAsync(request.RoomId);
                return new ResponseDto<bool>(true);
            }
            catch (Exception ex)
            {
                return new ResponseDto<bool>
                {
                    IsSucceeded = false,
                    Message = "An error occurred while deleting the room.",
                    Errors = new[] { ex.Message }
                };
            }
        }
    }
}
