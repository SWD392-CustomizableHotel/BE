using Entities;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.OtherObjects;

namespace SWD.SheritonHotel.Data.Repositories.Interfaces;

public interface IAccountRepository
{
    Task<(List<ApplicationUser>, int)> GetAccountsAsync(int pageNumber, int pageSize, AccountFilter accountFilter, string searchTerm);
    Task<ApplicationUser> GetAccountByIdAsync(string accountId);
    Task<ApplicationUser> UpdateAccountAsync(string accountId, AccountDto accountDto);
    Task SoftDeleteAccountAsync(string accountId);
}