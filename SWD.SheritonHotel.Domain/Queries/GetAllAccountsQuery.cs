using MediatR;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.OtherObjects;

namespace SWD.SheritonHotel.Domain.Queries;

public class GetAllAccountsQuery : IRequest<PagedResponse<List<AccountDto>>>
{
    public PaginationFilter PaginationFilter { get; set; }
    public AccountFilter AccountFilter { get; set; }
    public string SearchTerm { get; set; }

    public GetAllAccountsQuery(PaginationFilter paginationFilter, AccountFilter accountFilter, string searchTerm = null)
    {
        PaginationFilter = paginationFilter ?? new PaginationFilter();
        AccountFilter = accountFilter ?? new AccountFilter();
        SearchTerm = searchTerm;
    }
}