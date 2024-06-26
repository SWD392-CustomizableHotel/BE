using MediatR;
using SWD.SheritonHotel.Domain.DTO;

namespace SWD.SheritonHotel.Domain.Queries;

public class GetAccountDetailQuery : IRequest<AccountDto>
{
    public GetAccountDetailQuery(string accountId)
    {
        AccountId = accountId;
    }

    public string AccountId { get; }
}