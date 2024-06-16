using Dtos;
using Entities;
using SWD.SheritonHotel.Domain.DTO;

namespace Interfaces
{
    public interface IAuthService
    {
        Task<AuthServiceResponseDto> SeedRolesAsync();
        Task<AuthServiceResponseDto> RegisterAsync(RegisterDto registerDto);
        Task<AuthServiceResponseDto> LoginAsync(LoginDto loginDto);
        Task<AuthServiceResponseDto> MakeAdminAsync(UpdatePermissionDto updatePermissionDto);
        Task<AuthServiceResponseDto> MakeStaffAsync(UpdatePermissionDto updatePermissionDto);
        Task<BaseResponse<ApplicationUser>> ResetPassword(string email, string token, string password);
    }
}
