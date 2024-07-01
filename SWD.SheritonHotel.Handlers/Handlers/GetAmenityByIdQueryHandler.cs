using Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
    public class GetAmenityByIdQueryHandler : IRequestHandler<GetAmenityByIdQuery, ResponseDto<Amenity>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAmenityService _amenityService;

        public GetAmenityByIdQueryHandler(IAmenityService amenityService, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _amenityService = amenityService;
        }

        public async Task<ResponseDto<Amenity>> Handle(GetAmenityByIdQuery request, CancellationToken cancellationToken)
        {

            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            if (user == null || !(await _userManager.IsInRoleAsync(user, "ADMIN")))
            {
                return new ResponseDto<Amenity>
                {
                    IsSucceeded = false,
                    Message = "Unauthorized",
                    Errors = new[] { "You must be an admin to perform this operation." }
                };
            }
            try
            {
                var amenity = await _amenityService.GetAmenityByIdAsync(request.AmenityId);
                return new ResponseDto<Amenity>
                {
                    IsSucceeded = true,
                    Message = "Room details fetched successfully",
                    Data = amenity,
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<Amenity>
                {
                    IsSucceeded = false,
                    Message = "An error occurred while getting Amenity Details",
                    Errors = new[] { ex.Message }
                };
            }
        }
    }
}
