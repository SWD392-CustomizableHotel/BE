using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SWD.SheritonHotel.Domain.Commands.AmenityCommand;
using SWD.SheritonHotel.Domain.DTO.Responses;
using SWD.SheritonHotel.Domain.Entities;
using SWD.SheritonHotel.Services;
using SWD.SheritonHotel.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Handlers.Handlers.AmenityHandler.CommandsHandler
{
    public class DeleteAmenityCommandHandler : IRequestHandler<DeleteAmenityCommand, ResponseDto<bool>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAmenityService _amenityService;

        public DeleteAmenityCommandHandler(IAmenityService amenityService, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _amenityService = amenityService;
        }
        public async Task<ResponseDto<bool>> Handle(DeleteAmenityCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            if (user == null || !await _userManager.IsInRoleAsync(user, "Admin"))
            {
                return new ResponseDto<bool>
                {
                    IsSucceeded = false,
                    Message = "Unauthorized",
                    Errors = new[] { "You must be an admin to perform this operation." }
                };
            }
            try
            {
                await _amenityService.DeleteAmenityAsync(request.AmenityId);
                return new ResponseDto<bool>(true);
            }
            catch (Exception ex)
            {
                return new ResponseDto<bool>
                {
                    IsSucceeded = false,
                    Message = "An error occurred while deleting the amenity.",
                    Errors = new[] { ex.Message }
                };
            }
        }
    }
}
