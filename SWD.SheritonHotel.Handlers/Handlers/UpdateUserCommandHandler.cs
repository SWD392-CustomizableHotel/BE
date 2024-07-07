using Entities;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using SWD.SheritonHotel.Data.Repositories.Interfaces;
using SWD.SheritonHotel.Domain.Commands;
using SWD.SheritonHotel.Domain.DTO;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Domain.Handlers
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, BaseResponse<ApplicationUser>>
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public UpdateUserCommandHandler(
            IWebHostEnvironment hostingEnvironment, 
            IUserRepository userRepository, 
            UserManager<ApplicationUser> userManager)
        {
            _hostingEnvironment = hostingEnvironment;
            _userRepository = userRepository;
            _userManager = userManager;
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
            user.Dob = request.Dob.HasValue ? request.Dob.Value.ToDateTime(new TimeOnly(0, 0)) : null;
            user.PhoneNumber = request.PhoneNumber;
            user.Address = request.Address;

            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("STAFF") && request.Certificate != null && request.Certificate.Length > 0)
            {
                var uploadPath = Path.Combine(@"C:\Ky 7\SWD\BE\SWD.SheritonHotel.Domain\Certificate");

                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                var fileName = $"{user.Id}";
                var filePath = Path.Combine(uploadPath, fileName);

                try
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await request.Certificate.CopyToAsync(stream);
                    }

                    if (File.Exists(filePath))
                    {
                        user.CertificatePath = filePath;
                    }
                }
                catch (Exception ex)
                {
                    return new BaseResponse<ApplicationUser>
                    {
                        IsSucceed = false,
                        Message = $"Exception while saving certificate: {ex.Message}"
                    };
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
}
