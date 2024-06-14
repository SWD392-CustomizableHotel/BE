using Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SWD.SheritonHotel.Data.Repositories.Interfaces;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Handlers.Handlers
{
    public class GetRoomDetailsQueryHandler : IRequestHandler<GetRoomDetailsQuery, ResponseDto<RoomDto>>
    {
        private readonly IRoomRepository _roomRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetRoomDetailsQueryHandler(IRoomRepository roomRepository, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _roomRepository = roomRepository;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseDto<RoomDto>> Handle(GetRoomDetailsQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            if (user == null || !(await _userManager.IsInRoleAsync(user, "Admin")))
            {
                return new ResponseDto<RoomDto>
                {
                    IsSucceeded = false,
                    Message = "Unauthorized",
                    Errors = new[] { "You must be an admin to perform this operation." }
                };
            }
            try
            {
                var room = await _roomRepository.GetRoomByIdAsync(request.RoomId);

                var roomDto = new RoomDto
                {
                    RoomId = room.Id,
                    RoomNumber = room.Code,
                    RoomType = room.Type,
                    RoomStatus = room.Status,
                    RoomPrice = room.Price,
                    IsDeleted = room.IsDeleted
                };

                return new ResponseDto<RoomDto>
                {
                    IsSucceeded = true,
                    Message = "Room details fetched successfully",
                    Data = roomDto,
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<RoomDto>
                {
                    IsSucceeded = false,
                    Message = "An error occurred while getting room detail",
                    Errors = new[] { ex.Message }
                };
            }
            throw new NotImplementedException();
        }
    }
}
