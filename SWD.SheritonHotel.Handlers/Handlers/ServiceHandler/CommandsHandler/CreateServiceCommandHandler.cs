using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SWD.SheritonHotel.Data.Repositories;
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
    public class CreateServiceCommandHandler : IRequestHandler<CreateServiceCommand, ResponseDto<Service>>
    {
        private readonly IManageService _manageService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IValidator<CreateServiceCommand> _validator;

        public CreateServiceCommandHandler(IManageService manageService, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, IValidator<CreateServiceCommand> validator)
        {
            _manageService = manageService;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _validator = validator;
        }

        public async Task<ResponseDto<Service>> Handle(CreateServiceCommand request, CancellationToken cancellationToken)
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
            if (user == null || !await _userManager.IsInRoleAsync(user, StaticUserRoles.ADMIN))
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
                StartDate = request.StartDate,
                EndDate = request.EndDate,
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
