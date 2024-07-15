using Entities;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SWD.SheritonHotel.Domain.Commands;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.Entities;
using SWD.SheritonHotel.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Handlers.Handlers
{
    public class UploadIdentityCardCommandHandler : IRequestHandler<UploadIdentityCardCommand, ResponseDto<IdentityCardDto>>
    {
        private readonly IIdentityCardService _identityCardService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IValidator<UploadIdentityCardCommand> _validator;

        public UploadIdentityCardCommandHandler(
            IIdentityCardService identityCardService,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            IValidator<UploadIdentityCardCommand> validator)
        {
            _identityCardService = identityCardService;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _validator = validator;
        }

        public async Task<ResponseDto<IdentityCardDto>> Handle(UploadIdentityCardCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            if (user == null || !(await _userManager.IsInRoleAsync(user, "STAFF")))
            {
                return new ResponseDto<IdentityCardDto>
                {
                    IsSucceeded = false,
                    Message = "Unauthorized",
                    Errors = new[] { "You must be a staff to perform this operation." }
                };
            }

            ValidationResult result = await _validator.ValidateAsync(request, cancellationToken);
            if (!result.IsValid)
            {
                return new ResponseDto<IdentityCardDto>
                {
                    IsSucceeded = false,
                    Message = "Validation Error",
                    Errors = result.Errors.Select(e => e.ErrorMessage).ToArray()
                };
            }

            try
            {
                var identityCardDto = await _identityCardService.UploadIdentityCardAsync(request.FrontFile, request.PaymentId, cancellationToken);

                if (identityCardDto != null)
                {
                    return new ResponseDto<IdentityCardDto>
                    {
                        IsSucceeded = true,
                        Message = "Upload Identity Card Successfully",
                        Data = identityCardDto
                    };
                }
                else
                {
                    return new ResponseDto<IdentityCardDto>
                    {
                        IsSucceeded = false,
                        Message = "Upload Identity Card Failed"
                    };
                }
            }
            catch (Exception ex)
            {
                var innerException = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return new ResponseDto<IdentityCardDto>
                {
                    IsSucceeded = false,
                    Message = "An error occurred while uploading the identity card.",
                    Errors = new[] { innerException }
                };
            }
        }

        private string GenerateRandomCode()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
        }
    }
}
