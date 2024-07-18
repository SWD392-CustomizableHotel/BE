using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SWD.SheritonHotel.Data.Repositories.Interfaces;
using SWD.SheritonHotel.Domain.Commands.RoomCommand;
using SWD.SheritonHotel.Domain.DTO.Responses;
using SWD.SheritonHotel.Domain.Entities;
using SWD.SheritonHotel.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Handlers.Handlers.RoomHandler.CommandsHandler
{
    public class EditRoomDetailsCommandHandler : IRequestHandler<EditRoomDetailsCommand, ResponseDto<Room>>
    {
        private readonly IRoomService _roomService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EditRoomDetailsCommandHandler(IRoomService roomService, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _roomService = roomService;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ResponseDto<Room>> Handle(EditRoomDetailsCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            if (user == null || !await _userManager.IsInRoleAsync(user, "ADMIN"))
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
                var room = await _roomService.UpdateRoomAsync(request.RoomId, request.RoomType, request.RoomPrice, request.ImageFile, user.UserName);
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
