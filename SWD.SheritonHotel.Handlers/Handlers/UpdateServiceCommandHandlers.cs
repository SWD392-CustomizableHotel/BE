using Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWD.SheritonHotel.Services.Interfaces;
using OtherObjects;
using FluentValidation;
using FluentValidation.Results;

namespace SWD.SheritonHotel.Handlers.Handlers
{
    public class UpdateServiceCommandHandler : IRequestHandler<UpdateServiceCommand, ResponseDto<Service>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IManageService _manageService;
        private readonly IValidator<UpdateServiceCommand> _validator;

        public UpdateServiceCommandHandler(IManageService manageService, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, IValidator<UpdateServiceCommand> validator)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _manageService = manageService;
            _validator = validator;
        }

        public async Task<ResponseDto<Service>> Handle(UpdateServiceCommand request, CancellationToken cancellationToken)
        {
            // Validate the command 
            ValidationResult result = await _validator.ValidateAsync(request, cancellationToken);
            if (!result.IsValid)
            {
                return new ResponseDto<Service>
                {
                    IsSucceeded = false,
                    Message = "Validation Error",
                    Errors = result.Errors.Select(e => e.ErrorMessage).ToArray()
                };
            }

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
            try
            {
                var updatedService = await _manageService.UpdateServiceAsync(request.ServiceId, request.Name,
                    request.Description, request.Price, request.StartDate.Date, request.EndDate.Date, user.UserName);
                return new ResponseDto<Service>(updatedService);
            }
            catch (Exception ex)
            {
                return new ResponseDto<Service>
                {
                    IsSucceeded = false,
                    Message = "An error occurred while updating the Service.",
                    Errors = new[] { ex.Message }
                };
            }
        }
    }
}
