using Entities;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SWD.SheritonHotel.Domain.Commands;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Services.Interfaces;
namespace SWD.SheritonHotel.Handlers.Handlers
{
    public class CreateAmenityCommandHandler : IRequestHandler<CreateAmenityCommand, ResponseDto<Amenity>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAmenityService _amenityService;
        private readonly IValidator<CreateAmenityCommand> _validator;


        public CreateAmenityCommandHandler(IAmenityService amenityService, UserManager<ApplicationUser> userManager, 
            IHttpContextAccessor httpContextAccessor, IValidator<CreateAmenityCommand> validator)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _amenityService = amenityService;
            _validator = validator;
        }

        public async Task<ResponseDto<Amenity>> Handle(CreateAmenityCommand request, CancellationToken cancellationToken)
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

            ValidationResult result = await _validator.ValidateAsync(request, cancellationToken);
            if (!result.IsValid)
            {
                return new ResponseDto<Amenity>
                {
                    IsSucceeded = false,
                    Message = "Validation Error",
                    Errors = result.Errors.Select(e => e.ErrorMessage).ToArray()
                };
            }

            var code = GenerateAmenityCode();
            var amenity = new Amenity
            {
                Name = request.Name,
                Description = request.Description,
                HotelId = request.HotelId,
                Price = request.Price,
                Status = request.Status,
                IsDeleted = false,
                Code  = code,
                CreatedBy = user.UserName,
                LastUpdatedBy = user.UserName,
                CreatedDate = DateTime.UtcNow,
            };
            try
            {
                var newAmenity = await _amenityService.CreateAmenityAsync(amenity);
                return new ResponseDto<Amenity>(newAmenity);
            }
            catch (Exception ex)
            {
                return new ResponseDto<Amenity>
                {
                    IsSucceeded = false,
                    Message = "An error occurred while creating the room.",
                    Errors = new[] { ex.Message }
                };
            }
        }

        private string GenerateAmenityCode()
        {
            return Guid.NewGuid().ToString().Replace("-", "").Substring(0, 6).ToUpper();
        }
    }
}
