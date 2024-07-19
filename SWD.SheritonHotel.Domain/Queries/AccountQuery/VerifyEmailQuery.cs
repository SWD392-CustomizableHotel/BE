using MediatR;
using SWD.SheritonHotel.Domain.DTO.Responses;

namespace SWD.SheritonHotel.Domain.Queries.AccountQuery
{
    public class VerifyEmailQuery : IRequest<BaseResponse<object>>
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
