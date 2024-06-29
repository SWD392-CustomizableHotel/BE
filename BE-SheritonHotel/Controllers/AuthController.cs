using System.ComponentModel.DataAnnotations;
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

namespace Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly EmailSender _sender;
    private readonly IUserService _userService;

    public AuthController(IAuthService authService, IUserService userService, IMediator mediator, EmailSender sender,
        IMapper mapper)
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

        if (!ModelState.IsValid) return NotFound();

        return Ok(users);
    }

    // Route -> Register
    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody][Required] RegisterDto registerDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(new BaseResponse<object>
            { IsSucceed = false, Message = "Invalid model state", Result = null });

        var authServiceResponse = await _authService.RegisterAsync(registerDto);

        if (authServiceResponse.IsSucceed)
            return Ok(new BaseResponse<object>
            {
                IsSucceed = true,
                Message = "Account created successfully and check your email to verify account!",
                Result = null
            });
        return BadRequest(new BaseResponse<object>
        { IsSucceed = false, Message = authServiceResponse.Token, Result = null });
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
            return Ok(new BaseResponse<ApplicationUser>
            { IsSucceed = false, Result = null, Message = "Cannot find your email. Please input correct email" });

        var token = await _mediator.Send(new GenerateResetPasswordCommand { User = user });

        try
        {
            if (!_sender.SendForgotPasswordEmail(user.Email, token))
                return Ok(new BaseResponse<ApplicationUser> { IsSucceed = false, Message = "Failed to send email" });
        }
        catch (Exception ex)
        {
            return Ok(new BaseResponse<ApplicationUser>
            { IsSucceed = false, Result = null, Message = "Failed to send email or update user" });
        }

        return Ok(new BaseResponse<ApplicationUser>
        {
            IsSucceed = true,
            Result = null,
            Message = "Successfully request forgot password. Please check your email"
        });
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
            return Ok(new BaseResponse<ApplicationUser>
            { IsSucceed = false, Result = null, Message = "Failed to reset the password" });
        }
    }

    //verify
    [HttpGet]
    [Route("verify-email")]
    public async Task<IActionResult> VerifyEmail(string email, string token)
    {
        try
        {
            var result = await _userService.VerifyEmailTokenAsync(email, token);

            if (result)
                return Ok(new BaseResponse<object>
                { IsSucceed = true, Message = "Email verified successfully.", Result = null });
            return BadRequest(new BaseResponse<object>
            { IsSucceed = false, Message = "Invalid or expired token.", Result = null });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new BaseResponse<object> { IsSucceed = false, Message = ex.Message, Result = null });
        }
    }

    [HttpPost]
    [Route("RegisterAdditionalInfo")]
    public async Task<IActionResult> RegisterAdditionalInfo([FromBody] RegisterAdditionalInfoDto additionalInfoDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var response = await _authService.RegisterAdditionalInfoAsync(additionalInfoDto);
        if (response.IsSucceed) return Ok(response);

        return BadRequest(response);
    }

    [HttpGet]
    [Route("CheckUserRegistrationStatus")]
    public async Task<IActionResult> CheckUserRegistrationStatus([FromQuery] string idToken)
    {
        try
        {
            var response = await _authService.CheckUserRegistrationStatus(idToken);
            return Ok(response);
        }
        catch (Exception ex)
        {
            // Log the exception or handle it appropriately
            return StatusCode(StatusCodes.Status500InternalServerError, "Error checking user registration status");
        }
    }

    [HttpPost]
    [Route("ExternalLogin")]
    public async Task<IActionResult> LoginWithGoogle([FromBody] AuthGoogleDto authGoogleDto)
    {
        var payload = await _authService.VerifyGoogleToken(authGoogleDto);
        if (payload == null)
            return BadRequest("Invalid External Authentication.");

        var user = await _authService.FindByLoginOrEmailAsync(authGoogleDto.Provider, payload.Subject, payload.Email);

        if (user == null) return BadRequest("Invalid External Authentication.");

        var token = await _authService.GenerateToken(user);
        return Ok(new AuthServiceResponseDto { Token = token, IsSucceed = true });
    }
}