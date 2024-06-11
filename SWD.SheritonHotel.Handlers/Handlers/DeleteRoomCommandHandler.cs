using Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SWD.SheritonHotel.Data.Repositories.Interfaces;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.Commands;

namespace SWD.SheritonHotel.Handlers.Handlers
{
    public class DeleteRoomCommandHandler : IRequestHandler<DeleteRoomCommand, ResponseDto<bool>>
    {
        private readonly IRoomRepository _roomRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeleteRoomCommandHandler(IRoomRepository roomRepository, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _roomRepository = roomRepository;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseDto<bool>> Handle(DeleteRoomCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            if (user == null || !(await _userManager.IsInRoleAsync(user, "Admin")))
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
                await _roomRepository.DeleteRoomAsync(request.RoomId);
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
