using MediatR;
using SWD.SheritonHotel.Domain.DTO.Account;
using SWD.SheritonHotel.Domain.DTO.Responses;

namespace SWD.SheritonHotel.Domain.Queries.AccountQuery;

public class GetAccountDetailQuery : IRequest<ResponseDto<AccountDto>>
{
    public GetAccountDetailQuery(string accountId)
    {
        AccountId = accountId;
    }

    public string AccountId { get; }
}