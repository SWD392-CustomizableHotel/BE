using AutoMapper;
using Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SWD.SheritonHotel.Data.Repositories.Interfaces;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.Queries;
using SWD.SheritonHotel.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Handlers.Handlers
{
    public class GetRoomDetailsQueryHandler : IRequestHandler<GetRoomDetailsQuery, ResponseDto<RoomDetailsDTO>>
    {
        private readonly IRoomService _roomService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public GetRoomDetailsQueryHandler(IRoomService roomService, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _roomService = roomService;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<ResponseDto<RoomDetailsDTO>> Handle(GetRoomDetailsQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            try
            {
                var room = await _roomService.GetRoomByIdAsync(request.RoomId);
                var roomDetailsDto = _mapper.Map<RoomDetailsDTO>(room);
                roomDetailsDto.HotelAddress = room.Hotel?.Address;
                return new ResponseDto<RoomDetailsDTO>
                {
                    IsSucceeded = true,
                    Message = "Room details fetched successfully",
                    Data = roomDetailsDto,
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<RoomDetailsDTO>
                {
                    IsSucceeded = false,
                    Message = "An error occurred while getting room detail",
                    Errors = new[] { ex.Message }
                };
            }
        }
    }
}
