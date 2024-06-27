using MediatR;
using SWD.SheritonHotel.Domain.DTO;

namespace SWD.SheritonHotel.Domain.Commands;

public class UpdateAccountDetailCommand : IRequest<ResponseDto<AccountDto>>
{
    public string AccountId { get; set; }
    public AccountDto AccountDto { get; set; }


    public UpdateAccountDetailCommand(string accountId, AccountDto accountDto)
    {
    AccountId = accountId;
    AccountDto = accountDto;
    }
}