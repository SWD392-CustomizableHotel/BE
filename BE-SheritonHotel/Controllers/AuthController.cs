using Dtos;
using Entities;
using Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.Queries;
using SWD.SheritonHotel.Domain.Utilities;
using System.ComponentModel.DataAnnotations;

namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IMediator _mediator;
        private readonly EmailSender _sender;

        public AuthController(IAuthService authService, IUserService userService, IMediator mediator, EmailSender sender)
        {
            _authService = authService;
            _userService = userService;
            _mediator = mediator;
            _sender = sender;
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

            if(!ModelState.IsValid)
            {
                return NotFound();
            }

            return Ok(users);
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
        [Route("forgot-password")]
        public async Task<IActionResult> ForgotPassword([Required] [FromBody] GetUserByEmailQuery query)
        {
            var user = await _mediator.Send(query);
            if (user == null)
            {
                return Ok(new BaseResponse<ApplicationUser>() { IsSuccess = false, Result = null, Message = "Email is not found" });
            }

            user.PasswordResetToken = TokenGenerator.CreateRandomToken();
            user.ResetTokenExpires = DateTime.Now.AddMinutes(5);
            
            try
            {
                await _userService.UpdateAsync(user);
                if (!_sender.SendForgotPasswordEmail(user.Email, user.PasswordResetToken))
                {
                    return Ok(new BaseResponse<ApplicationUser>() { IsSuccess = false, Message = "Failed to send email" });
                }
            } catch (Exception ex)
            {
                return Ok(new BaseResponse<ApplicationUser>() { IsSuccess = false, Result = null, Message = "Failed to send email or update user" });
            }
            return Ok(new BaseResponse<ApplicationUser>() { IsSuccess = true, Result = null, Message = "Successfully request forgot password" });
        }
    }
}

