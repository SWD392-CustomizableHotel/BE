using Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SWD.SheritonHotel.Domain.Commands;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.OtherObjects;
using SWD.SheritonHotel.Services;
using SWD.SheritonHotel.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Handlers.Handlers
{
    public class UpdateAmenityStatusCommandHandler : IRequestHandler<UpdateAmenityStatusCommand, ResponseDto<Amenity>>
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAmenityService _amenityService;

        public UpdateAmenityStatusCommandHandler(IAmenityService amenityService, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _amenityService = amenityService;
        }
        public async Task<ResponseDto<Amenity>> Handle(UpdateAmenityStatusCommand request, CancellationToken cancellationToken)
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

            if (!Enum.TryParse<AmenityStatus>(request.Status.ToString(), true, out var status))
            {
                return new ResponseDto<Amenity>
                {
                    IsSucceeded = false,
                    Message = "Invalid status value",
                    Errors = new[] { "The provided status value is not valid." }
                };
            }
            try
            {
                var updatedAmenity = await _amenityService.UpdateAmenityStatus(request.AmenityId, request.Status.ToString(), user.UserName);
                return new ResponseDto<Amenity>(updatedAmenity);
            }
            catch (Exception ex)
            {
                return new ResponseDto<Amenity>
                {
                    IsSucceeded = false,
                    Message = "An error occurred while updating the Amenity status.",
                    Errors = new[] { ex.Message }
                };
            }
        }
    }
}
