﻿using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Dtos;
using Entities;
using Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using OtherObjects;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.Utilities;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace Services;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly EmailVerify _emailVerify;
    private readonly IConfigurationSection _goolgeSettings;
    private readonly IConfigurationSection _jwtSettings;
    private readonly ILogger<AuthService> _logger;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        ILogger<AuthService> logger,
        IConfiguration configuration,
        EmailVerify emailVerify,
        TokenGenerator tokenGenerator
    )
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _jwtSettings = _configuration.GetSection("JWT");
        _goolgeSettings = _configuration.GetSection("GoogleAuthSettings");
        _logger = logger;
        _emailVerify = emailVerify;
    }

    public async Task<AuthServiceResponseDto> LoginAsync(LoginDto loginDto)
    {
        var user = await _userManager.FindByNameAsync(loginDto.UserName);

        if (user is null)
            return new AuthServiceResponseDto
            {
                IsSucceed = false,
                Token = "Invalid Credentials"
            };
        if (!user.isActived)
            return new AuthServiceResponseDto
            {
                IsSucceed = false,
                Token = "Account not verified!"
            };

        var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginDto.Password);

        if (!isPasswordCorrect)
            return new AuthServiceResponseDto
            {
                IsSucceed = false,
                Token = "Invalid Credentials"
            };

        var userRoles = await _userManager.GetRolesAsync(user);

        var authClaims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.NameIdentifier, user.Id),
            new("JWTID", Guid.NewGuid().ToString()),
            new("FirstName", user.FirstName),
            new("LastName", user.LastName)
        };

        foreach (var userRole in userRoles) authClaims.Add(new Claim(ClaimTypes.Role, userRole));

        var token = GenerateNewJsonWebToken(authClaims);

        return new AuthServiceResponseDto { IsSucceed = true, Token = token };
    }

    public async Task<AuthServiceResponseDto> MakeAdminAsync(
        UpdatePermissionDto updatePermissionDto
    )
    {
        var user = await _userManager.FindByNameAsync(updatePermissionDto.UserName);

        if (user is null)
            return new AuthServiceResponseDto
            {
                IsSucceed = false,
                Token = "Invalid User name!!!!!!!!"
            };
        var roles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, roles.ToArray());

        await _userManager.AddToRoleAsync(user, StaticUserRoles.ADMIN);

        return new AuthServiceResponseDto
        {
            IsSucceed = true,
            Token = "User is now an ADMIN"
        };
    }

    public async Task<AuthServiceResponseDto> MakeStaffAsync(
        UpdatePermissionDto updatePermissionDto
    )
    {
        var user = await _userManager.FindByNameAsync(updatePermissionDto.UserName);

        if (user is null)
            return new AuthServiceResponseDto
            {
                IsSucceed = false,
                Token = "Invalid User name!!!!!!!!"
            };
        var roles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, roles.ToArray());

        await _userManager.AddToRoleAsync(user, StaticUserRoles.STAFF);

        return new AuthServiceResponseDto
        {
            IsSucceed = true,
            Token = "User is now an STAFF"
        };
    }

    public async Task<AuthServiceResponseDto> RegisterAsync(RegisterDto registerDto)
    {
        var isExistsUser = await _userManager.FindByNameAsync(registerDto.UserName);

        if (isExistsUser != null)
            return new AuthServiceResponseDto
            {
                IsSucceed = false,
                Token = "UserName Already Exists"
            };

        // Check if email is already in use
        var isExistsEmail = await _userManager.FindByEmailAsync(registerDto.Email);
        if (isExistsEmail != null)
            return new AuthServiceResponseDto
            {
                IsSucceed = false,
                Token = "Email Already Exists"
            };

        if (registerDto.Password != registerDto.ConfirmPassword)
            return new AuthServiceResponseDto
            {
                IsSucceed = false,
                Token = "The password and confirmation password do not match."
            };

        var newUser = new ApplicationUser
        {
            Id = Guid.NewGuid().ToString(),
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            Email = registerDto.Email,
            UserName = registerDto.UserName,
            SecurityStamp = Guid.NewGuid().ToString(),
            VerifyTokenExpires = DateTime.Now.AddHours(24)
        };

        var createUserResult = await _userManager.CreateAsync(newUser, registerDto.Password);

        if (!createUserResult.Succeeded)
        {
            var errorString = "User Creation Failed Because: " +
                              string.Join(" # ", createUserResult.Errors.Select(e => e.Description));
            return new AuthServiceResponseDto { IsSucceed = false, Token = errorString };
        }

        // Add a Default USER Role to all users
        await _userManager.AddToRoleAsync(newUser, StaticUserRoles.CUSTOMER);

        // Generate verification token using custom TokenGenerator
        var verificationToken = TokenGenerator.CreateRandomToken();
        newUser.VerifyToken = verificationToken;

        // Update user with verification token
        var updateUserResult = await _userManager.UpdateAsync(newUser);
        if (!updateUserResult.Succeeded)
        {
            var errorString = "User Update Failed Because: " +
                              string.Join(" # ", updateUserResult.Errors.Select(e => e.Description));
            return new AuthServiceResponseDto { IsSucceed = false, Token = errorString };
        }

        // Send verification email
        var emailSent = _emailVerify.SendVerifyAccountEmail(newUser.Email, verificationToken);
        if (!emailSent)
            return new AuthServiceResponseDto
            {
                IsSucceed = false,
                Token = "Email sending failed!"
            };

        return new AuthServiceResponseDto
        {
            IsSucceed = true,
            Token = "Account created successfully and check your email to verify account! "
        };
    }

    public async Task<BaseResponse<ApplicationUser>> ResetPassword(string email, string token, string newPassword)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(s => s.Email.ToLower().Equals(email.ToLower()));
        if (user == null)
            return new BaseResponse<ApplicationUser>
            {
                IsSucceed = false,
                Message = "User email is not found."
            };

        var resetPasswordResult = await _userManager.ResetPasswordAsync(user, token, newPassword);
        if (!resetPasswordResult.Succeeded)
            return new BaseResponse<ApplicationUser>
            {
                IsSucceed = false,
                Message = "Your request has been expired, please try request again."
            };

        return new BaseResponse<ApplicationUser>
        {
            IsSucceed = true,
            Message = "Password reset successfully.",
            Result = user
        };
    }

    public async Task<AuthServiceResponseDto> SeedRolesAsync()
    {
        var isOwnerRoleExists = await _roleManager.RoleExistsAsync(StaticUserRoles.STAFF);
        var isAdminRoleExists = await _roleManager.RoleExistsAsync(StaticUserRoles.ADMIN);
        var isUserRoleExists = await _roleManager.RoleExistsAsync(StaticUserRoles.CUSTOMER);

        if (isOwnerRoleExists && isAdminRoleExists && isUserRoleExists)
            return new AuthServiceResponseDto
            {
                IsSucceed = true,
                Token = "Roles Seeding is Already Done"
            };

        await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.CUSTOMER));
        await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.ADMIN));
        await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.STAFF));

        return new AuthServiceResponseDto
        {
            IsSucceed = true,
            Token = "Role Seeding Done Successfully"
        };
    }

    public async Task<string> GenerateToken(ApplicationUser user)
    {
        var signingCredentials = GetSigningCredentials();
        var claims = await GetClaims(user);
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
        var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        return token;
    }

    public async Task<Payload> VerifyGoogleToken(AuthGoogleDto authGoogleDto)
    {
        try
        {
            var settings = new ValidationSettings
            {
                Audience = new List<string> { _goolgeSettings.GetSection("ClientId").Value }
            };

            var payload = await ValidateAsync(authGoogleDto.IdToken, settings);

            return payload;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<AuthServiceResponseDto> CheckUserRegistrationStatus(string idToken)
    {
        var authGoogleDto = new AuthGoogleDto { IdToken = idToken };
        var payload = await VerifyGoogleToken(authGoogleDto);

        if (payload == null)
        {
            Console.WriteLine("Google token verification failed.");
            return new AuthServiceResponseDto() { IsSucceed = false, Token = null };
        }

        var normalizedEmail = payload.Email.Trim().ToLower();

        var user = await _userManager.Users.SingleOrDefaultAsync(u => u.NormalizedEmail.Trim().ToLower() == normalizedEmail);
        if (user == null)
        {
            Console.WriteLine($"User with email {normalizedEmail} not found. Needs to register additional info.");
            return new AuthServiceResponseDto() { IsSucceed = false, Token = null };
        }

        if (!user.isActived)
        {
            return new AuthServiceResponseDto() { IsSucceed = false, Token = "Account not verified!" };
        }

        if (string.IsNullOrEmpty(user.FirstName) || string.IsNullOrEmpty(user.LastName) || string.IsNullOrEmpty(user.PhoneNumber))
        {
            Console.WriteLine("User has not completed additional information.");
            return new AuthServiceResponseDto() { IsSucceed = false, Token = null };
        }

        var userRoles = await _userManager.GetRolesAsync(user);
        var role = userRoles.FirstOrDefault() ?? StaticUserRoles.CUSTOMER;
        var authClaims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.NameIdentifier, user.Id),
            new("JWTID", Guid.NewGuid().ToString()),
            new("FirstName", user.FirstName),
            new("LastName", user.LastName),
            new(ClaimTypes.Role, role)
        };

        foreach (var userRole in userRoles) authClaims.Add(new Claim(ClaimTypes.Role, userRole));

        var token = GenerateNewJsonWebToken(authClaims);

        return new AuthServiceResponseDto() { IsSucceed = true, Token = token, Role = role };
    }


    public async Task<AuthServiceResponseDto> RegisterAdditionalInfoAsync(RegisterAdditionalInfoDto additionalInfoDto)
    {
        var normalizedEmail = additionalInfoDto.UserName.Trim().ToLower();
        var user = await _userManager.Users.SingleOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail.ToUpper());

        //var user = await _userManager.FindByNameAsync(normalizedEmail);
        if (user == null)
        {
            user = new ApplicationUser
            {
                Email = additionalInfoDto.UserName,
                UserName = additionalInfoDto.UserName,
                NormalizedUserName = additionalInfoDto.UserName.ToUpper(),
                NormalizedEmail = normalizedEmail.ToUpper(),
                Id = Guid.NewGuid().ToString(),
                FirstName = additionalInfoDto.FirstName,
                LastName = additionalInfoDto.LastName,
                PhoneNumber = additionalInfoDto.PhoneNumber,
                isActived = true
            };

            var createUserResult = await _userManager.CreateAsync(user);
            if (!createUserResult.Succeeded)
            {
                var errorString = "User Creation Failed Because: ";
                foreach (var error in createUserResult.Errors) errorString += " # " + error.Description;

                return new AuthServiceResponseDto { IsSucceed = false, Token = errorString };
            }

            await _userManager.AddToRoleAsync(user, StaticUserRoles.CUSTOMER);
        }
        else
        {
            user.FirstName = additionalInfoDto.FirstName;
            user.LastName = additionalInfoDto.LastName;
            user.PhoneNumber = additionalInfoDto.PhoneNumber;
            user.isActived = true;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
                return new AuthServiceResponseDto { IsSucceed = false, Token = "Failed to update user" };
        }

        var token = await GenerateToken(user);
        var userRoles = await _userManager.GetRolesAsync(user);
        var role = userRoles.FirstOrDefault() ?? StaticUserRoles.CUSTOMER;

        return new AuthServiceResponseDto { IsSucceed = true, Token = token, Role = role };
    }

    public async Task<ApplicationUser> FindByLoginOrEmailAsync(string loginProvider, string providerKey, string email)
    {
        var user = await _userManager.FindByLoginAsync(loginProvider, providerKey);
        if (user == null) user = await _userManager.FindByEmailAsync(email);
        return user;
    }

    private string GenerateNewJsonWebToken(List<Claim> claims)
    {
        var authSecret = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])
        );
        var expires = DateTime.UtcNow.AddDays(7);

        var tokenObject = new JwtSecurityToken(
            _configuration["JWT:ValidIssuer"],
            _configuration["JWT:ValidAudience"],
            expires: DateTime.Now.AddHours(1),
            claims: claims,
            signingCredentials: new SigningCredentials(
                authSecret,
                SecurityAlgorithms.HmacSha256
            )
        );

        var token = new JwtSecurityTokenHandler().WriteToken(tokenObject);

        return token;
    }

    private SigningCredentials GetSigningCredentials()
    {
        var key = Encoding.UTF8.GetBytes(_jwtSettings.GetSection("Secret").Value);
        var secret = new SymmetricSecurityKey(key);

        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    private async Task<List<Claim>> GetClaims(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Email)
        };

        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles) claims.Add(new Claim(ClaimTypes.Role, role));

        return claims;
    }

    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {
        var tokenOptions = new JwtSecurityToken(
            _jwtSettings["ValidIssuer"],
            _jwtSettings["ValidAudience"],
            claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtSettings["expiryInMinutes"])),
            signingCredentials: signingCredentials);

        return tokenOptions;
    }
}