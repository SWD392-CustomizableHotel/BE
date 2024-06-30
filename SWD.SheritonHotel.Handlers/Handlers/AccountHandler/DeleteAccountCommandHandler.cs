using Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SWD.SheritonHotel.Domain.Commands;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Services.Interfaces;

namespace SWD.SheritonHotel.Handlers.Handlers;

public class DeleteAccountCommandHandler : IRequestHandler<DeleteAccountCommand, ResponseDto<bool>>
{
    private readonly IAccountService _accountService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DeleteAccountCommandHandler(IAccountService accountService, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
    {
        _accountService = accountService;
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ResponseDto<bool>> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

        try
        {
            await _accountService.SoftDeleteAccountAsync(request.AccountId);
            return new ResponseDto<bool>(true);
        }
        catch (Exception ex)
        {
            return new ResponseDto<bool>
            {
                IsSucceeded = false,
                Message = "An error occurred while deleting the account.",
                Errors = new[] { ex.Message }
            };
        }
    }
}