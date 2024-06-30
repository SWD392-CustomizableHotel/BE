using Dtos;
using Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OtherObjects;
using SWD.SheritonHotel.Data.Context;
using SWD.SheritonHotel.Domain.Commands;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.Utilities;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthServiceResponseDto>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly EmailVerify _emailVerify;
    private readonly TokenGenerator _tokenGenerator;
    private readonly ApplicationDbContext _context;

    public RegisterCommandHandler(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        EmailVerify emailVerify,
        TokenGenerator tokenGenerator,
        ApplicationDbContext context)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _emailVerify = emailVerify;
        _tokenGenerator = tokenGenerator;
        _context = context;
    }

    public async Task<AuthServiceResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var registerDto = request.RegisterDto;

        // Check if the username already exists
        var isExistsUser = await _userManager.FindByNameAsync(registerDto.UserName);
        if (isExistsUser != null)
        {
            Console.WriteLine("Username already exists");
            return new AuthServiceResponseDto()
            {
                IsSucceed = false,
                Token = "UserName Already Exists"
            };
        }

        // Check if the email is already in use using a direct query
        var isExistsEmail = await _userManager.FindByEmailAsync(registerDto.Email);
        if (isExistsEmail != null)
        {
            Console.WriteLine("Email already exists");
            return new AuthServiceResponseDto()
            {
                IsSucceed = false,
                Token = "Email Already Exists"
            };
        }

        Console.WriteLine("Email does not exist, proceeding with registration");

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
            Token = "Account created successfully and check your email to verify account!"
        };
    }

}


