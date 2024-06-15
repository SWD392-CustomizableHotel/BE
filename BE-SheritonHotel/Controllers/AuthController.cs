
using AutoMapper;
using Dtos;
using Entities;
using Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWD.SheritonHotel.Domain.Commands;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.Queries;
using SWD.SheritonHotel.Domain.Utilities;
using SWD.SheritonHotel.Handlers.Handlers;
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
        private readonly IMapper _mapper;
        private readonly EmailSender _sender;

        public AuthController(IAuthService authService, IUserService userService, IMediator mediator, EmailSender sender, IMapper mapper)
        {
            _authService = authService;
            _userService = userService;
            _mediator = mediator;
            _sender = sender;
            _mapper = mapper;
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
        public async Task<IActionResult> ForgotPassword([Required][FromBody] GetUserByEmailQuery query)
        {
            var user = await _mediator.Send(query);
            if (user == null)
            {
                return Ok(new BaseResponse<ApplicationUser>() { IsSucceed = false, Result = null, Message = "Cannot find your email. Please input correct email" });
            }

            var token = await _mediator.Send(new GenerateResetPasswordCommand() { User = user });

            try
            {
                if (!_sender.SendForgotPasswordEmail(user.Email, token))
                {
                    return Ok(new BaseResponse<ApplicationUser>() { IsSucceed = false, Message = "Failed to send email" });
                }
            }
            catch (Exception ex)
            {
                return Ok(new BaseResponse<ApplicationUser>() { IsSucceed = false, Result = null, Message = "Failed to send email or update user" });
            }
            return Ok(new BaseResponse<ApplicationUser>() { IsSucceed = true, Result = null, Message = "Successfully request forgot password. Please check your email" });
        }

        [HttpPost]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword([Required][FromBody] ResetPasswordQuery query)
        {
            try
            {
                var response = await _mediator.Send(query);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new BaseResponse<ApplicationUser>() { IsSucceed = false, Result = null, Message = "Failed to reset the password" });
            }
        }
    }
}
