using System.Linq.Dynamic.Core;
using AutoMapper;
using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SWD.SheritonHotel.Data.Base;
using SWD.SheritonHotel.Data.Context;
using SWD.SheritonHotel.Data.Repositories.Interfaces;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.OtherObjects;

namespace SWD.SheritonHotel.Data.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager; 

        public AccountRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<(List<ApplicationUser>, int)> GetAccountsAsync(int pageNumber, int pageSize, AccountFilter accountFilter, string searchTerm)
        {
            var accounts = _context.Users.AsQueryable();

            accounts = accounts.Where(a => a.isActived);
            
            if (!string.IsNullOrEmpty(accountFilter.UserName))
            { 
                accounts = accounts.Where(a => a.UserName.Contains(accountFilter.UserName));
            }

            if (!string.IsNullOrEmpty(accountFilter.Email))
            {
                accounts = accounts.Where(a => a.Email.Contains(accountFilter.Email));
            }
            
            if (!string.IsNullOrEmpty(searchTerm))
            {
                accounts = accounts.Where(a => a.UserName.Contains(searchTerm) ||
                                               a.Email.Contains(searchTerm) ||
                                               a.FirstName.Contains(searchTerm) ||
                                               a.LastName.Contains(searchTerm));
            }
            
            var totalRecords = await accounts.CountAsync();
            var paginatedAccounts = await accounts.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return (paginatedAccounts, totalRecords);
        }

        public async Task<ApplicationUser> GetAccountByIdAsync(string accountId)
        {
            return await _context.Users.FindAsync(accountId);
        }

        public async Task<ApplicationUser> UpdateAccountAsync(string accountId, AccountDto accountDto = null)
        {
            var account = await _context.Users.FindAsync(accountId);
            if (account == null)
            {
                throw new KeyNotFoundException($"No account found with ID {accountId}");
            }
            
            account.UserName = accountDto.UserName;
            account.Email = accountDto.Email;
            account.PhoneNumber = accountDto.PhoneNumber;
            account.FirstName = accountDto.FirstName;
            account.LastName = accountDto.LastName;
            account.Dob = accountDto.Dob;
            
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
            
            await _context.SaveChangesAsync();
            return account;
        }

        public async Task SoftDeleteAccountAsync(string accountId)
        {
            var account = await _context.Users
                .Include(u => u.AssignedServices)
                .FirstOrDefaultAsync(u => u.Id == accountId);
            
            if (account == null)
            {
                throw new KeyNotFoundException($"No account found with ID {accountId}");
            }
            
            if (account.AssignedServices.Any())
            {
                _context.AssignedServices.RemoveRange(account.AssignedServices);
            }
            
            account.isActived = false;
            await _context.SaveChangesAsync();
        }
}