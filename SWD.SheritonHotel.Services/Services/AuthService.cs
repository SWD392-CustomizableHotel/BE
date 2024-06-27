using System.IdentityModel.Tokens.Jwt;
using System.Linq.Dynamic.Core.Tokenizer;
using System.Security.Claims;
using System.Text;
using Dtos;
using Entities;
using Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OtherObjects;
using SWD.SheritonHotel.Domain.Utilities;
using SWD.SheritonHotel.Domain.DTO;

namespace Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly EmailVerify _emailVerify;
        private readonly TokenGenerator _tokenGenerator;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            EmailVerify emailVerify,
            TokenGenerator tokenGenerator
        )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _emailVerify = emailVerify;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<AuthServiceResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.UserName);

            if (user is null)
                return new AuthServiceResponseDto()
                {
                    IsSucceed = false,
                    Token = "Invalid Credentials"
                };
            if (!user.isActived)
            {
                return new AuthServiceResponseDto()
                {
                    IsSucceed = false,
                    Token = "Account not verified!"
                };
            }

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!isPasswordCorrect)
                return new AuthServiceResponseDto()
                {
                    IsSucceed = false,
                    Token = "Invalid Credentials"
                };

            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim("JWTID", Guid.NewGuid().ToString()),
                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName),
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var token = GenerateNewJsonWebToken(authClaims);

            return new AuthServiceResponseDto() { IsSucceed = true, Token = token };
        }

        public async Task<AuthServiceResponseDto> MakeAdminAsync(
            UpdatePermissionDto updatePermissionDto
        )
        {
            var user = await _userManager.FindByNameAsync(updatePermissionDto.UserName);

            if (user is null)
                return new AuthServiceResponseDto()
                {
                    IsSucceed = false,
                    Token = "Invalid User name!!!!!!!!"
                };
            var roles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, roles.ToArray());

            await _userManager.AddToRoleAsync(user, StaticUserRoles.ADMIN);

            return new AuthServiceResponseDto()
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
                return new AuthServiceResponseDto()
                {
                    IsSucceed = false,
                    Token = "Invalid User name!!!!!!!!"
                };
            var roles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, roles.ToArray());

            await _userManager.AddToRoleAsync(user, StaticUserRoles.STAFF);

            return new AuthServiceResponseDto()
            {
                IsSucceed = true,
                Token = "User is now an STAFF"
            };
        }

        public async Task<AuthServiceResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            var isExistsUser = await _userManager.FindByNameAsync(registerDto.UserName);

            if (isExistsUser != null)
            {
                return new AuthServiceResponseDto()
                {
                    IsSucceed = false,
                    Token = "UserName Already Exists"
                };
            }

            // Check if email is already in use
            var isExistsEmail = await _userManager.FindByEmailAsync(registerDto.Email);
            if (isExistsEmail != null)
            {
                return new AuthServiceResponseDto()
                {
                    IsSucceed = false,
                    Token = "Email Already Exists"
                };
            }

            if (registerDto.Password != registerDto.ConfirmPassword)
            {
                return new AuthServiceResponseDto()
                {
                    IsSucceed = false,
                    Token = "The password and confirmation password do not match."
                };
            }

            ApplicationUser newUser = new ApplicationUser()
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
                return new AuthServiceResponseDto() { IsSucceed = false, Token = errorString };
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
                return new AuthServiceResponseDto() { IsSucceed = false, Token = errorString };
            }
            // Send verification email
            bool emailSent = _emailVerify.SendVerifyAccountEmail(newUser.Email, verificationToken);
            if (!emailSent)
            {
                return new AuthServiceResponseDto()
                {
                    IsSucceed = false,
                    Token = "Email sending failed!"
                };
            }

            return new AuthServiceResponseDto()
            {
                IsSucceed = true,
                Token = "Account created successfully and check your email to verify account! "
            };
        }

        public async Task<BaseResponse<ApplicationUser>> ResetPassword(string email, string token, string newPassword)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(s => s.Email.ToLower().Equals(email.ToLower()));
            if (user == null)
            {
                return new BaseResponse<ApplicationUser>
                {
                    IsSucceed = false,
                    Message = "User email is not found."
                };
            }

            var resetPasswordResult = await _userManager.ResetPasswordAsync(user, token, newPassword);
            if (!resetPasswordResult.Succeeded)
            {
                return new BaseResponse<ApplicationUser>
                {
                    IsSucceed = false,
                    Message = "Your request has been expired, please try request again."
                };
            }

            return new BaseResponse<ApplicationUser>
            {
                IsSucceed = true,
                Message = "Password reset successfully.",
                Result = user
            };
        }

        public async Task<AuthServiceResponseDto> SeedRolesAsync()
        {
            bool isOwnerRoleExists = await _roleManager.RoleExistsAsync(StaticUserRoles.STAFF);
            bool isAdminRoleExists = await _roleManager.RoleExistsAsync(StaticUserRoles.ADMIN);
            bool isUserRoleExists = await _roleManager.RoleExistsAsync(StaticUserRoles.CUSTOMER);

            if (isOwnerRoleExists && isAdminRoleExists && isUserRoleExists)
                return new AuthServiceResponseDto()
                {
                    IsSucceed = true,
                    Token = "Roles Seeding is Already Done"
                };

            await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.CUSTOMER));
            await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.ADMIN));
            await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.STAFF));

            return new AuthServiceResponseDto()
            {
                IsSucceed = true,
                Token = "Role Seeding Done Successfully"
            };
        }

        private string GenerateNewJsonWebToken(List<Claim> claims)
        {
            var authSecret = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])
            );

            var tokenObject = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(1),
                claims: claims,
                signingCredentials: new SigningCredentials(
                    authSecret,
                    SecurityAlgorithms.HmacSha256
                )
            );

            string token = new JwtSecurityTokenHandler().WriteToken(tokenObject);

            return token;
        }
    }
}