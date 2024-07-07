using MediatR;
using SWD.SheritonHotel.Domain.DTO;

namespace SWD.SheritonHotel.Domain.Commands;

public class DeleteAccountCommand : IRequest<ResponseDto<bool>>
{
    public string AccountId { get; set; }
}