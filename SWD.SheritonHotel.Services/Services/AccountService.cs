using AutoMapper;
using Entities;
using Microsoft.AspNetCore.Identity;
using SWD.SheritonHotel.Data.Context;
using SWD.SheritonHotel.Data.Repositories.Interfaces;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.OtherObjects;
using SWD.SheritonHotel.Services.Interfaces;

namespace SWD.SheritonHotel.Services.Services;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;
    

    public AccountService(IAccountRepository accountRepository, IMapper mapper, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
    {
        _accountRepository = accountRepository;
        _mapper = mapper;
        _userManager = userManager;
        _context = context;
    }

    public async Task<(List<AccountDto>, int)> GetAccountsAsync(int pageNumber, int pageSize, AccountFilter accountFilter, string searchTerm = null)
    {
        var (accounts, totalRecords) = await _accountRepository.GetAccountsAsync(pageNumber, pageSize, accountFilter, searchTerm);

        var accountDtos = new List<AccountDto>();
        foreach (var account in accounts)
        {
            var roles = await _userManager.GetRolesAsync(account);
            var accountDto = _mapper.Map<AccountDto>(account);
            accountDto.Roles = roles.ToList();
            accountDtos.Add(accountDto);
        }

        return (accountDtos, totalRecords);
    }

    public async Task<AccountDto> GetAccountByIdAsync(string accountId)
    {
        var account = await _accountRepository.GetAccountByIdAsync(accountId);
        if (account == null) return null;
        
        var accountDto = _mapper.Map<AccountDto>(account);
        var roles = await _userManager.GetRolesAsync(account);
        accountDto.Roles = roles.ToList();

        return accountDto;
    }

    public async Task<AccountDto> UpdateAccountAsync(string accountId, AccountDto accountDto)
{
    var account = await _accountRepository.GetAccountByIdAsync(accountId);
    if (account == null)
    {
        throw new KeyNotFoundException($"No account found with ID {accountId}");
    }

    // Update account details
    account.UserName = accountDto.UserName;
    account.Email = accountDto.Email;
    account.PhoneNumber = accountDto.PhoneNumber;
    account.FirstName = accountDto.FirstName;
    account.LastName = accountDto.LastName;
    account.Dob = accountDto.Dob;
    account.isActived = accountDto.IsActive;

    // Update roles if necessary
    var currentRoles = await _userManager.GetRolesAsync(account);
    var newRoles = accountDto.Roles;

    var rolesToAdd = newRoles.Except(currentRoles).ToList();
    var rolesToRemove = currentRoles.Except(newRoles).ToList();

    // Ensure roles to add or remove are only CUSTOMER and STAFF roles
    var allowedRoles = new List<string> { "CUSTOMER", "STAFF" };

    if (rolesToAdd.Any(r => !allowedRoles.Contains(r)))
    {
        throw new InvalidOperationException("Only 'CUSTOMER' and 'STAFF' roles can be added.");
    }

    if (rolesToRemove.Any(r => !allowedRoles.Contains(r)))
    {
        throw new InvalidOperationException("Only 'CUSTOMER' and 'STAFF' roles can be removed.");
    }

    // Ensure no promotion to ADMIN
    if (newRoles.Contains("ADMIN"))
    {
        throw new InvalidOperationException("Promotion to 'ADMIN' role is not allowed.");
    }

    if (rolesToAdd.Count > 0)
    {
        await _userManager.AddToRolesAsync(account, rolesToAdd);
    }

    if (rolesToRemove.Count > 0)
    {
        await _userManager.RemoveFromRolesAsync(account, rolesToRemove);
    }

    // Save changes
    await _context.SaveChangesAsync();

    var updatedAccountDto = _mapper.Map<AccountDto>(account);
    updatedAccountDto.Roles = (List<string>)await _userManager.GetRolesAsync(account);

    return updatedAccountDto;
}

    public async Task SoftDeleteAccountAsync(string accountId)
    { 
        await _accountRepository.SoftDeleteAccountAsync(accountId);
    }
}