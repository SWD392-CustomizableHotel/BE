using Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SWD.SheritonHotel.Data.Repositories.Interfaces;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.Commands;


namespace SWD.SheritonHotel.Handlers.Handlers
{
    public class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand, ResponseDto<int>>
    {
        private readonly IRoomRepository _roomRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateRoomCommandHandler(IRoomRepository roomRepository, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _roomRepository = roomRepository;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseDto<int>> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            if (user == null || !(await _userManager.IsInRoleAsync(user, "ADMIN")))
            {
                return new ResponseDto<int>
                {
                    IsSucceeded = false,
                    Message = "Unauthorized",
                    Errors = new[] { "You must be an admin to perform this operation." }
                };
            }
            var newRoom = new Room
            {
                Code = request.RoomNumber,
                Type = request.Type,
                Status = request.Status,
                Price = request.Price,
                Description = request.Description,
                HotelId = request.HotelId,
                CreatedBy = user.UserName,
                LastUpdatedBy = user.UserName,
            };

            try
            {
                var newRoomId = await _roomRepository.CreateRoomAsync(newRoom);
                return new ResponseDto<int>(newRoomId);
            }
            catch (Exception ex)
            {
                return new ResponseDto<int>
                {
                    IsSucceeded = false,
                    Message = "An error occurred while creating the room.",
                    Errors = new[] { ex.Message }
                };
            }
        }
    }
}
