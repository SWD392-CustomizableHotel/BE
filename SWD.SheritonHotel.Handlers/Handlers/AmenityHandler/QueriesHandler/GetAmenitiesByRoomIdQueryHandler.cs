using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SWD.SheritonHotel.Domain.DTO.Amenity;
using SWD.SheritonHotel.Domain.DTO.Responses;
using SWD.SheritonHotel.Domain.Entities;
using SWD.SheritonHotel.Domain.Queries.AmenityQuery;
using SWD.SheritonHotel.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Handlers.Handlers.AmenityHandler.QueriesHandler
{
    public class GetAmenitiesByRoomIdQueryHandler : IRequestHandler<GetAmenitiesByRoomIdQuery, ResponseDto<List<AmenityDTO>>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAmenityService _amenityService;
        private readonly IMapper _mapper;

        public GetAmenitiesByRoomIdQueryHandler(IAmenityService amenityService, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _amenityService = amenityService;
            _mapper = mapper;
        }
        public async Task<ResponseDto<List<AmenityDTO>>> Handle(GetAmenitiesByRoomIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            if (user == null || !await _userManager.IsInRoleAsync(user, "ADMIN"))
            {
                return new ResponseDto<List<AmenityDTO>>
                {
                    IsSucceeded = false,
                    Message = "Unauthorized",
                    Errors = new[] { "You must be an admin to perform this operation." }
                };
            }
            try
            {
                var amenities = await _amenityService.GetAmenitiesByRoomIdAsync(request.RoomId);
                var amenitiesDto = amenities.Select(amenity => new AmenityDTO
                {
                    Id = amenity.Id,
                    Name = amenity.Name,
                    Description = amenity.Description,
                    Price = amenity.Price,
                    Status = amenity.Status
                }).ToList();
                return new ResponseDto<List<AmenityDTO>>(amenitiesDto);
            }
            catch (Exception ex)
            {
                return new ResponseDto<List<AmenityDTO>>
                {
                    IsSucceeded = false,
                    Message = "An error occurred while seeing the amenites assigned to that room.",
                    Errors = new[] { ex.Message }
                };
            }
        }
    }
}
