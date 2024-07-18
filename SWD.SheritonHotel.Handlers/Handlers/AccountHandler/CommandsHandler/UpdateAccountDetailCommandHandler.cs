using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SWD.SheritonHotel.Data.Repositories.Interfaces;
using SWD.SheritonHotel.Domain.Commands.Auth;
using SWD.SheritonHotel.Domain.DTO.Account;
using SWD.SheritonHotel.Domain.DTO.Responses;
using SWD.SheritonHotel.Domain.Entities;

namespace SWD.SheritonHotel.Handlers.Handlers.AccountHandler.CommandsHandler;

public class UpdateAccountDetailCommandHandler : IRequestHandler<UpdateAccountDetailCommand, ResponseDto<AccountDto>>
{
    private readonly IAccountRepository _accountRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;

    public UpdateAccountDetailCommandHandler(IAccountRepository accountRepository, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, IMapper mapper)
    {
        _accountRepository = accountRepository;
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
    }

    public async Task<ResponseDto<AccountDto>> Handle(UpdateAccountDetailCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _accountRepository.GetAccountByIdAsync(request.AccountId);
            if (user == null)
            {
                return new ResponseDto<AccountDto>
                {
                    IsSucceeded = false,
                    Message = "Account not found",
                    Errors = new[] { "No account found with the provided ID." }
                };
            }

            // Update account details
            user.FirstName = request.AccountDto.FirstName;
            user.LastName = request.AccountDto.LastName;
            user.UserName = request.AccountDto.UserName;
            user.PhoneNumber = request.AccountDto.PhoneNumber;
            user.Email = request.AccountDto.Email;
            user.Dob = request.AccountDto.Dob;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return new ResponseDto<AccountDto>
                {
                    IsSucceeded = false,
                    Message = "Failed to update account",
                    Errors = result.Errors.Select(e => e.Description).ToArray()
                };
            }

            // Update roles if necessary
            var currentRoles = await _userManager.GetRolesAsync(user);
            var newRoles = request.AccountDto.Roles;
            var rolesToAdd = newRoles.Except(currentRoles).ToList();
            var rolesToRemove = currentRoles.Except(newRoles).ToList();

            // Ensure roles to add or remove are only CUSTOMER and STAFF roles
            var allowedRoles = new List<string> { "CUSTOMER", "STAFF" };
            if (rolesToAdd.Any(r => !allowedRoles.Contains(r)))
            {
                return new ResponseDto<AccountDto>
                {
                    IsSucceeded = false,
                    Message = "Invalid role update",
                    Errors = new[] { "Only 'CUSTOMER' and 'STAFF' roles can be added." }
                };
            }

            if (rolesToRemove.Any(r => !allowedRoles.Contains(r)))
            {
                return new ResponseDto<AccountDto>
                {
                    IsSucceeded = false,
                    Message = "Invalid role update",
                    Errors = new[] { "Only 'CUSTOMER' and 'STAFF' roles can be removed." }
                };
            }

            if (rolesToAdd.Count > 0)
            {
                var addRolesResult = await _userManager.AddToRolesAsync(user, rolesToAdd);
                if (!addRolesResult.Succeeded)
                {
                    return new ResponseDto<AccountDto>
                    {
                        IsSucceeded = false,
                        Message = "Failed to add roles",
                        Errors = addRolesResult.Errors.Select(e => e.Description).ToArray()
                    };
                }
            }

            if (rolesToRemove.Count > 0)
            {
                var removeRolesResult = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
                if (!removeRolesResult.Succeeded)
                {
                    return new ResponseDto<AccountDto>
                    {
                        IsSucceeded = false,
                        Message = "Failed to remove roles",
                        Errors = removeRolesResult.Errors.Select(e => e.Description).ToArray()
                    };
                }
            }

            var updatedAccountDto = _mapper.Map<ApplicationUser, AccountDto>(user);
            updatedAccountDto.Roles = (List<string>)await _userManager.GetRolesAsync(user);

            return new ResponseDto<AccountDto>
            {
                IsSucceeded = true,
                Message = "Account details and roles updated successfully",
                Data = updatedAccountDto
            };
        }
        catch (Exception ex)
        {
            return new ResponseDto<AccountDto>
            {
                IsSucceeded = false,
                Message = "An error occurred while updating the account details.",
                Errors = new[] { ex.Message }
            };
        }
    }
}