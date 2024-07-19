using MediatR;
using SWD.SheritonHotel.Domain.DTO.Responses;

namespace SWD.SheritonHotel.Domain.Commands.Auth;

public class DeleteAccountCommand : IRequest<ResponseDto<bool>>
{
    public string AccountId { get; set; }
}