using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SWD.SheritonHotel.Data.Repositories.Interfaces;
using SWD.SheritonHotel.Domain.DTO.Responses;
using SWD.SheritonHotel.Domain.Entities;
using SWD.SheritonHotel.Domain.OtherObjects;
using SWD.SheritonHotel.Domain.Queries.ServiceQuery;
using SWD.SheritonHotel.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Handlers.Handlers.ServiceHandler.QueriesHandler
{
    public class GetServiceByIdQueryHandler : IRequestHandler<GetServiceByIdQuery, ResponseDto<Service>>
    {
        private readonly IManageService _manageService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetServiceByIdQueryHandler(IManageService manageService, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _manageService = manageService;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseDto<Service>> Handle(GetServiceByIdQuery request, CancellationToken cancellationToken)
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
            try
            {
                var service = await _manageService.GetServiceByIdAsync(request.ServiceId);
                return new ResponseDto<Service>
                {
                    IsSucceeded = true,
                    Message = "Service details fetched successfully",
                    Data = service,
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<Service>
                {
                    IsSucceeded = false,
                    Message = "An error occurred while getting service details",
                    Errors = new[] { ex.Message }
                };
            }
        }
    }
}
