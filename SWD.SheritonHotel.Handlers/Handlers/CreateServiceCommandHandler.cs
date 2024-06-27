using Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using OtherObjects;
using SWD.SheritonHotel.Data.Repositories;
using SWD.SheritonHotel.Data.Repositories.Interfaces;
using SWD.SheritonHotel.Domain.Commands;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Handlers.Handlers
{
    public class CreateServiceCommandHandler : IRequestHandler<CreateServiceCommand, ResponseDto<Service>>
    {
        private readonly IManageService _manageService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateServiceCommandHandler(IManageService manageService, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _manageService = manageService;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseDto<Service>> Handle(CreateServiceCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            if (user == null || !(await _userManager.IsInRoleAsync(user, StaticUserRoles.ADMIN)))
            {
                return new ResponseDto<Service>
                {
                    IsSucceeded = false,
                    Message = "Unauthorized",
                    Errors = new[] { "You must be an admin to perform this operation." }
                };
            }

            var code = GenerateServiceCode();
            var service = new Service
            {
                Name = request.Name,
                Price = request.Price,
                Description = request.Description,
                Status = request.Status,
                Code = code,
                CreatedBy = user.UserName,
                CreatedDate = DateTime.UtcNow,
                LastUpdatedBy = user.UserName,
                HotelId = request.HotelId,
                IsDeleted = false
            };

            try
            {
                var newService = await _manageService.CreateServiceAsync(service);
                return new ResponseDto<Service>(newService);
            }
            catch (Exception ex)
            {
                return new ResponseDto<Service>
                {
                    IsSucceeded = false,
                    Message = "An error occurred while creating service.",
                    Errors = new[] { ex.Message }
                };
            }
        }

        private string GenerateServiceCode()
        {
            return Guid.NewGuid().ToString().Replace("-", "").Substring(0, 6).ToUpper();
        }
    }
}
