using Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SWD.SheritonHotel.Data.Repositories.Interfaces;
using SWD.SheritonHotel.Domain.Commands;
using SWD.SheritonHotel.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Handlers.Handlers
{
    public class EditRoomDetailsCommandHandler : IRequestHandler<EditRoomDetailsCommand, ResponseDto<Room>>
    {
        private readonly IRoomRepository _roomRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EditRoomDetailsCommandHandler(IRoomRepository roomRepository, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _roomRepository = roomRepository;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ResponseDto<Room>> Handle(EditRoomDetailsCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            if (user == null || !(await _userManager.IsInRoleAsync(user, "ADMIN")))
            {
                return new ResponseDto<Room>
                {
                    Data = new Room(),
                    IsSucceeded = false,
                    Message = "Unauthorized",
                    Errors = new[] { "You must be an admin to perform this operation." }
                };
            }

            try
            {
                var room = await _roomRepository.UpdateRoomAsync(request.RoomId, request.RoomType, request.RoomPrice);
                return new ResponseDto<Room>(room); 
            }
            catch (Exception ex)
            {
                return new ResponseDto<Room>
                {
                    IsSucceeded = false,
                    Message = "An error occurred while creating the room.",
                    Errors = new[] { ex.Message }
                };
            }

        }
    }
}
