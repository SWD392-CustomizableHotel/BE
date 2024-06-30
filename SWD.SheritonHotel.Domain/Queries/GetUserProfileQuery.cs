using Entities;
using MediatR;
using SWD.SheritonHotel.Domain.DTO;

namespace SWD.SheritonHotel.Domain.Queries
{
    public class GetUserProfileQuery : IRequest<BaseResponse<ApplicationUser>>
    {
        public string UserId { get; set; }

        public GetUserProfileQuery(string userId)
        {
            UserId = userId;
        }
    }
}