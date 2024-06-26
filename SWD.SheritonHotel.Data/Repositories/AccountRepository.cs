using System.Linq.Dynamic.Core;
using AutoMapper;
using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

        public async Task<(List<ApplicationUser>, int)> GetAccountsAsync(int pageNumber, int pageSize, AccountFilter accountFilter, string searchTerm = null)
        {
            var accounts = _context.Users.AsQueryable();

            accounts = accounts.Where(a => a.isActived);

            // Apply filtersw
            if (!string.IsNullOrEmpty(accountFilter.UserName))
            {
                accounts = accounts.Where(a => a.UserName.Contains(accountFilter.UserName));
            }

            if (!string.IsNullOrEmpty(accountFilter.Email))
            {
                accounts = accounts.Where(a => a.Email.Contains(accountFilter.Email));
            }
            

            // Apply search term
            if (!string.IsNullOrEmpty(searchTerm))
            {
                accounts = accounts.Where(a => a.UserName.Contains(searchTerm) ||
                                               a.Email.Contains(searchTerm) ||
                                               a.FirstName.Contains(searchTerm) ||
                                               a.LastName.Contains(searchTerm));
            }

            // Count total records that match the search term for pagination
            var totalRecords = await accounts.CountAsync();

            // Apply pagination
            var paginatedAccounts = await accounts.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return (paginatedAccounts, totalRecords);
        }

        public async Task<ApplicationUser> GetAccountByIdAsync(string accountId)
        {
            return await _context.Users.FindAsync(accountId);
        }

        public async Task<ApplicationUser> UpdateAccountAsync(string accountId, AccountDto accountDto)
        {
            var account = await _context.Users.FindAsync(accountId);
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
            // Save changes
            await _context.SaveChangesAsync();
            return account;
        }

        public async Task SoftDeleteAccountAsync(string accountId)
        {
            var account = await _context.Users.Include(u => u.AssignedServices).FirstOrDefaultAsync(u => u.Id == accountId);
    
            if (account == null)
            {
                throw new KeyNotFoundException($"No account found with ID {accountId}");
            }

            if (await _userManager.IsInRoleAsync(account, "Staff"))
            {
                // Check if the staff account is assigned to any services
                if (account.AssignedServices.Any())
                {
                    // Unassign the staff from all services
                    _context.AssignedServices.RemoveRange(account.AssignedServices);
                }
            }

            // Perform soft delete by setting IsActive to false
            account.isActived = false;
    
            await _context.SaveChangesAsync();
            
        }
}