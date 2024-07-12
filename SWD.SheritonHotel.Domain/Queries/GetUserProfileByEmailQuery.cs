using Entities;
using MediatR;
using SWD.SheritonHotel.Domain.DTO;

namespace SWD.SheritonHotel.Domain.Queries
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