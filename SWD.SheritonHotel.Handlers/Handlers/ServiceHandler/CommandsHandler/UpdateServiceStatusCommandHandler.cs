using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SWD.SheritonHotel.Data.Repositories.Interfaces;
using SWD.SheritonHotel.Domain.Commands.ServiceCommand;
using SWD.SheritonHotel.Domain.DTO.Responses;
using SWD.SheritonHotel.Domain.Entities;
using SWD.SheritonHotel.Domain.OtherObjects;
using SWD.SheritonHotel.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Handlers.Handlers.ServiceHandler.CommandsHandler
{
    public class UpdateServiceStatusCommandHandler : IRequestHandler<UpdateServiceStatusCommand, ResponseDto<Service>>
    {
        private readonly IManageService _manageService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UpdateServiceStatusCommandHandler(IManageService manageService, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _manageService = manageService;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseDto<Service>> Handle(UpdateServiceStatusCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            if (user == null || !await _userManager.IsInRoleAsync(user, StaticUserRoles.ADMIN))
            {
                return new ResponseDto<Service>
                {
                    IsSucceeded = false,
                    Message = "Unauthorized",
                    Errors = new[] { "You must be an admin to perform this operation." }
                };
            }

            if (!Enum.TryParse<ServiceStatus>(request.Status.ToString(), true, out var status))
            {
                return new ResponseDto<Service>
                {
                    IsSucceeded = false,
                    Message = "Invalid status value",
                    Errors = new[] { "The provided status value is not valid." }
                };
            }

            try
            {
                var updatedService = await _manageService.UpdateServiceStatus(request.ServiceId, request.Status.ToString(), user.UserName);
                return new ResponseDto<Service>(updatedService);
            }
            catch (Exception ex)
            {
                return new ResponseDto<Service>
                {
                    IsSucceeded = false,
                    Message = "An error occurred while updating the Service status.",
                    Errors = new[] { ex.Message }
                };
            }
        }
    }
}
