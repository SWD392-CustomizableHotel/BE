﻿using Google.Apis.Auth;
using SWD.SheritonHotel.Domain.DTO.Auth;
using SWD.SheritonHotel.Domain.DTO.Responses;
using SWD.SheritonHotel.Domain.Entities;

namespace Interfaces
{
    public interface IAuthService
    {
        Task<AuthServiceResponseDto> SeedRolesAsync();
        Task<AuthServiceResponseDto> RegisterAsync(RegisterDto registerDto);
        Task<AuthServiceResponseDto> LoginAsync(LoginDto loginDto);
        Task<AuthServiceResponseDto> MakeAdminAsync(UpdatePermissionDto updatePermissionDto);
        Task<AuthServiceResponseDto> MakeStaffAsync(UpdatePermissionDto updatePermissionDto);
        Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(AuthGoogleDto authGoogleDTO);
        Task<string> GenerateToken(ApplicationUser user);
        Task<AuthServiceResponseDto> CheckUserRegistrationStatus(string userId);
        Task<AuthServiceResponseDto> RegisterAdditionalInfoAsync(RegisterAdditionalInfoDto additionalInfoDto);
        Task<BaseResponse<ApplicationUser>> ResetPassword(string email, string token, string password);
        Task<ApplicationUser> FindByLoginOrEmailAsync(string loginProvider, string providerKey, string email);
    }
}