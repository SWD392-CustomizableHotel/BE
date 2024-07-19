using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using SWD.SheritonHotel.Data.Repositories.Interfaces;
using SWD.SheritonHotel.Domain.Commands.Auth;
using SWD.SheritonHotel.Domain.DTO.Responses;
using SWD.SheritonHotel.Domain.Entities;
using SWD.SheritonHotel.Services.Interfaces;
using System.Threading;
using System.Threading.Tasks;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, BaseResponse<ApplicationUser>>
{
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly IUserRepository _userRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IBlobStorageService _blobStorageService;

    public UpdateUserCommandHandler(
        IWebHostEnvironment hostingEnvironment,
        IUserRepository userRepository,
        UserManager<ApplicationUser> userManager,
        IBlobStorageService blobStorageService)
    {
        _hostingEnvironment = hostingEnvironment;
        _userRepository = userRepository;
        _userManager = userManager;
        _blobStorageService = blobStorageService;
    }

    public async Task<BaseResponse<ApplicationUser>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByEmailAsync(request.Email);
        if (user == null)
        {
            return new BaseResponse<ApplicationUser>
            {
                IsSucceed = false,
                Message = "User not found"
            };
        }

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.Dob = request.Dob;
        user.PhoneNumber = request.PhoneNumber;
        user.Address = request.Address;

        var roles = await _userManager.GetRolesAsync(user);

        if (roles.Contains("STAFF") && request.Certificate != null && request.Certificate.Length > 0)
        {
            var fileName = $"{user.Id}_{request.Certificate.FileName}";
            var contentType = request.Certificate.ContentType;

            using (var stream = request.Certificate.OpenReadStream())
            {
                user.CertificatePath = await _blobStorageService.UploadFileAsync(stream, fileName, contentType);
            }
        }

        var updateResult = await _userManager.UpdateAsync(user);
        if (updateResult.Succeeded)
        {
            return new BaseResponse<ApplicationUser>
            {
                IsSucceed = true,
                Result = user,
                Message = "User profile updated successfully"
            };
        }

        return new BaseResponse<ApplicationUser>
        {
            IsSucceed = false,
            Message = "Failed to update user profile"
        };
    }
}
