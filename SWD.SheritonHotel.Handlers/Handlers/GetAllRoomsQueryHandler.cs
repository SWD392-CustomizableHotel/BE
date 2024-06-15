using AutoMapper;
using Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SWD.SheritonHotel.Data.Repositories.Interfaces;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.Queries;
using SWD.SheritonHotel.Services.Interfaces;

namespace SWD.SheritonHotel.Handlers.Handlers
{
    public class GetAllRoomsQueryHandler : IRequestHandler<GetAllRoomsQuery, PagedResponse<List<RoomDto>>>
    {
        private readonly IRoomService _roomService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public GetAllRoomsQueryHandler(IRoomService roomService, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _roomService = roomService;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<PagedResponse<List<RoomDto>>> Handle(GetAllRoomsQuery request, CancellationToken cancellationToken)
        {
            
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            //Check User Admin 
            if (user == null || (await _userManager.IsInRoleAsync(user, "CUSTOMER")))
            {
                return new PagedResponse<List<RoomDto>>(new List<RoomDto>(), 0, 0);
            }

            //Filter
            var totalRooms = await _roomService.GetTotalRoomsCountAsync();
            var validFilter = request.PaginationFilter;
            var rooms = await _roomService.GetRoomsAsync(validFilter.PageNumber, validFilter.PageSize,
                request.RoomFilter, request.SearchTerm);

            var roomDtos = rooms.Select(room => new RoomDto
            {
                RoomId = room.Id,
                RoomNumber = room.Code,
                RoomType = room.Type,
                RoomStatus = room.Status,
                RoomPrice = room.Price,
                IsDeleted = room.IsDeleted
            }).ToList();

            var totalPages = (int)Math.Ceiling(totalRooms/ (double)validFilter.PageSize);
            var totalRecords = await _roomService.GetTotalRoomsCountAsync();
            var response = new PagedResponse<List<RoomDto>>(roomDtos, validFilter.PageNumber, validFilter.PageSize)
            {
                TotalRecords = totalRecords,
                TotalPages = totalPages,
            };
            return response;
        }
    }
}
