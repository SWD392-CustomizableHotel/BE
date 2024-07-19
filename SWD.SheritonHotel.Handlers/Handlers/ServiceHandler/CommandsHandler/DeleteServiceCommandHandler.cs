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
    public class DeleteServiceCommandHandler : IRequestHandler<DeleteServiceCommand, ResponseDto<bool>>
    {
        private readonly IManageService _manageService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeleteServiceCommandHandler(IManageService manageService, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _manageService = manageService;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseDto<bool>> Handle(DeleteServiceCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            if (user == null || !await _userManager.IsInRoleAsync(user, StaticUserRoles.ADMIN))
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
                await _manageService.DeleteServiceAsync(request.ServiceId);
                return new ResponseDto<bool>(true);
            }
            catch (Exception ex)
            {
                return new ResponseDto<bool>
                {
                    IsSucceeded = false,
                    Message = "An error occurred while deleting the service.",
                    Errors = new[] { ex.Message }
                };
            }
        }
    }
}
