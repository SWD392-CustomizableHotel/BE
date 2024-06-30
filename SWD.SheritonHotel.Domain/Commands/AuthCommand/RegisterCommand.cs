using Dtos;
using MediatR;
using SWD.SheritonHotel.Domain.DTO;

namespace SWD.SheritonHotel.Domain.Commands
{
    public class RegisterCommand : IRequest<AuthServiceResponseDto>
    {
        public RegisterDto RegisterDto { get; set; }
    }
}
