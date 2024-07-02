using MediatR;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.Queries;
using SWD.SheritonHotel.Services.Interfaces;

namespace SWD.SheritonHotel.Handlers.Handlers;

public class GetAllAccountsQueryHandler : IRequestHandler<GetAllAccountsQuery, PagedResponse<List<AccountDto>>>
{
    private readonly IAccountService _accountService;

    public GetAllAccountsQueryHandler(IAccountService accountService)
    {
        _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
    }

    public async Task<PagedResponse<List<AccountDto>>> Handle(GetAllAccountsQuery request, CancellationToken cancellationToken)
    {
        var (accounts, totalRecords) = await _accountService.GetAccountsAsync(request.PaginationFilter.PageNumber, request.PaginationFilter.PageSize, request.AccountFilter, request.SearchTerm);

        var response = new PagedResponse<List<AccountDto>>(accounts, request.PaginationFilter.PageNumber, request.PaginationFilter.PageSize)
        {
            TotalRecords = totalRecords,
            TotalPages = (int)Math.Ceiling(totalRecords / (double)request.PaginationFilter.PageSize)
        };
        
        return response;
    }
}
