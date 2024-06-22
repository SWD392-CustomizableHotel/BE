using Dtos;
using MediatR;

namespace SWD.SheritonHotel.Domain.Commands
{
    public class RegisterCommand : IRequest<AuthServiceResponseDto>
    {
        public RegisterDto RegisterDto { get; set; }
    }
}
