using Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SWD.SheritonHotel.Data.Repositories.Interfaces;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.Commands;

namespace SWD.SheritonHotel.Handlers.Handlers
{
    public class UpdateRoomStatusCommandHandler : IRequestHandler<UpdateRoomStatusCommand, ResponseDto<Room>>
    {
        private readonly IRoomRepository _roomRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UpdateRoomStatusCommandHandler(IRoomRepository roomRepository, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _roomRepository = roomRepository;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseDto<Room>> Handle(UpdateRoomStatusCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

            if (user == null || !(await _userManager.IsInRoleAsync(user, "ADMIN")))
            {
                return new ResponseDto<Room>
                {
                    IsSucceeded = false,
                    Message = "Unauthorized",
                    Errors = new[] { "You must be an admin to perform this operation." }
                };
            }

            try
            {
                var updatedRoom = await _roomRepository.UpdateRoomStatusAsync(request.RoomId, 
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
