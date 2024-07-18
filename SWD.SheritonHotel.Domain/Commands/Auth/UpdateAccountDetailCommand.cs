using MediatR;
using SWD.SheritonHotel.Domain.DTO.Account;
using SWD.SheritonHotel.Domain.DTO.Responses;

namespace SWD.SheritonHotel.Domain.Commands.Auth;

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