using MediatR;
using Microsoft.AspNetCore.Identity;
using SWD.SheritonHotel.Domain.DTO;
using Entities;
using System.Threading;
using System.Threading.Tasks;
using SWD.SheritonHotel.Domain.Queries;

namespace SWD.SheritonHotel.Domain.Handlers
{
    public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileByEmailQuery, BaseResponse<ApplicationUser>>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public GetUserProfileQueryHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<BaseResponse<ApplicationUser>> Handle(GetUserProfileByEmailQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return new BaseResponse<ApplicationUser>
                {
                    IsSucceed = false,
                    Message = "User not found"
                };
            }

            return new BaseResponse<ApplicationUser>
            {
                IsSucceed = true,
                Result = user,
                Message = "User profile fetched successfully"
            };
        }
    }
}