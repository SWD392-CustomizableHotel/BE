using Entities;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SWD.SheritonHotel.Data.Repositories.Interfaces;
using SWD.SheritonHotel.Domain.Commands;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.Entities;
using SWD.SheritonHotel.Services.Interfaces;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Services.Services
{
    public class IdentityCardService : IIdentityCardService
    {
        private readonly IIdentityCardRepository _identityCardRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IValidator<UploadIdentityCardCommand> _validator;

        public IdentityCardService(
            IIdentityCardRepository identityCardRepository,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            IValidator<UploadIdentityCardCommand> validator)
        {
            _identityCardRepository = identityCardRepository;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _validator = validator;
        }

        public async Task<ResponseDto<IdentityCardDto>> UploadIdentityCardWithValidationAsync(UploadIdentityCardCommand request, CancellationToken cancellationToken)
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
                var identityCardDto = await UploadIdentityCardAsync(request.FrontFile, request.PaymentId, cancellationToken);

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

        public async Task<IdentityCardDto> UploadIdentityCardAsync(IFormFile frontFile, int paymentId, CancellationToken cancellationToken)
        {
            return await _identityCardRepository.UploadIdentityCardAsync(frontFile, paymentId, cancellationToken);
        }

        public async Task<List<IdentityCardDto>> GetAllIdentityCardsAsync(CancellationToken cancellationToken)
        {
            return await _identityCardRepository.GetAllIdentityCardsAsync(cancellationToken);
        }

    }
}
