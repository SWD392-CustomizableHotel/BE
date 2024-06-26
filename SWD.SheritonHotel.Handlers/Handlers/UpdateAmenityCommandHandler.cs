using Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SWD.SheritonHotel.Domain.Commands;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.OtherObjects;
using SWD.SheritonHotel.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Handlers.Handlers
{
    public class UpdateAmenityCommandHandler : IRequestHandler<UpdateAmenityCommand, ResponseDto<Amenity>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAmenityService _amenityService;

        public UpdateAmenityCommandHandler(IAmenityService amenityService, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _amenityService = amenityService;
        }

        public async Task<ResponseDto<Amenity>> Handle(UpdateAmenityCommand request, CancellationToken cancellationToken)
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
                var updatedAmenity = await _amenityService.UpdateAmenityAsync(request.AmenityId, request.Name,
                    request.Description, request.Price, user.UserName);
                return new ResponseDto<Amenity>(updatedAmenity);
            }
            catch (Exception ex)
            {
                return new ResponseDto<Amenity>
                {
                    IsSucceeded = false,
                    Message = "An error occurred while updating the Amenity.",
                    Errors = new[] { ex.Message }
                };
            }
        }
    }
}
