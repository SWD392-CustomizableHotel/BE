using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Dtos;
using Entities;
using Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OtherObjects;

namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthController(IAuthService authService, IUserService userService,
            UserManager<ApplicationUser> userManager)
        {
            _authService = authService;
            _userService = userService;
            _userManager = userManager;
        }

        // Route For Seeding my roles to DB
        [HttpPost]
        [Route("seed-roles")]
        public async Task<IActionResult> SeedRoles()
        {
            var seerRoles = await _authService.SeedRolesAsync();

            return Ok(seerRoles);
        }

        [HttpGet]
        [Authorize]
        [Route("users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetAllUsers();

            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            return Ok();
        }

        // Route -> Register
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var registerResult = await _authService.RegisterAsync(registerDto);

            if (registerResult.IsSucceed)
                return Ok(registerResult);

            return BadRequest(registerResult);
        }


        // Route -> Login
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var loginResult = await _authService.LoginAsync(loginDto);

            if (loginResult.IsSucceed)
                return Ok(loginResult);

            return BadRequest(new { message = "Username or password is incorrect" });
        }


        // Route -> make customer -> admin
        [HttpPost]
        [Route("make-admin")]
        public async Task<IActionResult> MakeAdmin([FromBody] UpdatePermissionDto updatePermissionDto)
        {
            var operationResult = await _authService.MakeAdminAsync(updatePermissionDto);

            if (operationResult.IsSucceed)
                return Ok(operationResult);

            return BadRequest(operationResult);
        }

        // Route -> make customer -> staff
        [HttpPost]
        [Route("make-staff")]
        public async Task<IActionResult> MakeOwner([FromBody] UpdatePermissionDto updatePermissionDto)
        {
            var operationResult = await _authService.MakeStaffAsync(updatePermissionDto);

            if (operationResult.IsSucceed)
                return Ok(operationResult);

            return BadRequest(operationResult);
        }

        [HttpPost]
        [Route("ExternalLogin")]
        public async Task<IActionResult> LoginWithGoogle([FromBody] AuthGoogleDto authGoogleDto)
        {
            var payload = await _authService.VerifyGoogleToken(authGoogleDto);
            if (payload == null)
                return BadRequest("Invalid External Authentication.");

            var info = new UserLoginInfo(authGoogleDto.Provider, payload.Subject, authGoogleDto.Provider);

            var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(payload.Email);
                if (user == null)
                {
                    user = new ApplicationUser
                        { Email = payload.Email, UserName = payload.Email, Id = Guid.NewGuid().ToString() };
                    await _userManager.CreateAsync(user);

                    //prepare and send an email for the email confirmation
                    await _userManager.AddToRoleAsync(user, StaticUserRoles.CUSTOMER);
                    await _userManager.AddLoginAsync(user, info);
                }
                else
                {
                    await _userManager.AddLoginAsync(user, info);
                }
            }

            if (user == null)
                return BadRequest("Invalid External Authentication.");

            var token = await _authService.GenerateToken(user);
            return Ok(new AuthServiceResponseDto { Token = token, IsSucceed = true });
        }

        [HttpGet]
        [Route("CheckUserRegistrationStatus")]
        public async Task<IActionResult> CheckUserRegistrationStatus([FromQuery] string idToken)
        {
            try
            {
                var isRegistered = await _authService.CheckUserRegistrationStatus(idToken);
                return Ok(isRegistered);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(StatusCodes.Status500InternalServerError, "Error checking user registration status");
            }
        }

        [HttpPost]
        [Route("RegisterAdditionalInfo")]
        public async Task<IActionResult> RegisterAdditionalInfo([FromBody] RegisterAdditionalInfoDto additionalInfoDto)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _authService.RegisterAdditionalInfoAsync(additionalInfoDto);
            if (response.IsSucceed)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
    }
}