using Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SWD.SheritonHotel.Data.Repositories.Interfaces;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.Queries;

namespace SWD.SheritonHotel.Handlers.Handlers
{
    public class GetAllRoomsQueryHandler : IRequestHandler<GetAllRoomsQuery, PagedResponse<List<RoomDto>>>
    {
        private readonly IRoomRepository _roomRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetAllRoomsQueryHandler(IRoomRepository roomRepository, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _roomRepository = roomRepository;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
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
            var totalRooms = await _roomRepository.GetTotalRoomsCountAsync();
            var validFilter = request.PaginationFilter;
            var rooms = await _roomRepository.GetRoomsAsync(validFilter.PageNumber, validFilter.PageSize, 
                request.RoomFilter, request.SearchTerm);


            var roomDtos = rooms.Select(room => new RoomDto
            {
                RoomNumber = room.Code,
                RoomType = room.Type,
                RoomStatus = room.Status,
                RoomPrice = room.Price,
            }).ToList();

            var totalPages = (int)Math.Ceiling(totalRooms/ (double)validFilter.PageSize);

            var response = new PagedResponse<List<RoomDto>>(roomDtos, validFilter.PageNumber, validFilter.PageSize)
            {
                TotalPages = totalPages,
            };
            return response;
        }
    }
}
