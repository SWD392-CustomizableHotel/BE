using Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SWD.SheritonHotel.Data.Repositories.Interfaces;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.Commands;
using SWD.SheritonHotel.Services.Interfaces;
using FluentValidation;
using FluentValidation.Results;


namespace SWD.SheritonHotel.Handlers.Handlers
{
    public class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand, ResponseDto<int>>
    {
        private readonly IRoomService _roomService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IValidator<CreateRoomCommand> _validator;

        public CreateRoomCommandHandler(IRoomService roomService, UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor, IValidator<CreateRoomCommand> validator)
        {
            _roomService = roomService;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _validator = validator;
        }

        public async Task<ResponseDto<int>> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
        {
            ValidationResult validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                return new ResponseDto<int>
                {
                    IsSucceeded = false,
                    Message = "Validation failed",
                    Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToArray()
                };
            }
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            if (user == null || !(await _userManager.IsInRoleAsync(user, "ADMIN")))
            {
                return new ResponseDto<int>
                {
                    IsSucceeded = false,
                    Message = "Unauthorized",
                    Errors = new[] { "You must be an admin to perform this operation." }
                };
            }
            var newRoom = new Room
            {
                Code = request.RoomNumber,
                Type = request.Type,
                Status = request.Status,
                Price = request.Price,
                Description = request.Description,
                HotelId = request.HotelId,
                CreatedBy = user.UserName,
                LastUpdatedBy = user.UserName,
                CreatedDate = DateTime.UtcNow,
            };

            try
            {
                var newRoomId = await _roomService.CreateRoomAsync(newRoom, request.ImageUpload);
                return new ResponseDto<int>(newRoomId);
            }
            catch (Exception ex)
            {
                return new ResponseDto<int>
                {
                    IsSucceeded = false,
                    Message = "An error occurred while creating the room.",
                    Errors = new[] { ex.Message }
                };
            }
        }
    }
}
