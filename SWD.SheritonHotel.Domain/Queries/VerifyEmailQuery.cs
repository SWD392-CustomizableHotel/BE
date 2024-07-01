using MediatR;
using SWD.SheritonHotel.Domain.DTO;

namespace SWD.SheritonHotel.Domain.Queries
{
    public class VerifyEmailQuery : IRequest<BaseResponse<object>>
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
