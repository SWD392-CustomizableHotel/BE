using MediatR;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.DTO.Auth;

namespace SWD.SheritonHotel.Domain.Commands.Auth
{
    public class RegisterCommand : IRequest<AuthServiceResponseDto>
    {
        public RegisterDto RegisterDto { get; set; }
    }
}
