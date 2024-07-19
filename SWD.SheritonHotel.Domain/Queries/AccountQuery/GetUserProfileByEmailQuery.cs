using MediatR;
using SWD.SheritonHotel.Domain.DTO.Responses;
using SWD.SheritonHotel.Domain.Entities;

namespace SWD.SheritonHotel.Domain.Queries.AccountQuery
{
    public class GetUserProfileByEmailQuery : IRequest<BaseResponse<ApplicationUser>>
    {
        public string Email { get; set; }

        public GetUserProfileByEmailQuery(string email)
        {
            Email = email;
        }
    }
}