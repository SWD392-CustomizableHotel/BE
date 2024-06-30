using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.OtherObjects;

namespace SWD.SheritonHotel.Services.Interfaces;

public interface IAccountService
{
    Task<(List<AccountDto>, int)> GetAccountsAsync(int pageNumber, int pageSize, AccountFilter accountFilter, string searchTerm = null);
    Task<AccountDto> GetAccountByIdAsync(string accountId);

    Task<AccountDto> UpdateAccountAsync(string accountId, AccountDto accountDto);
    Task SoftDeleteAccountAsync(string accountId);
}