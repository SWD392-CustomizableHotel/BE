using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using SWD.SheritonHotel.Domain.DTO.Responses;
using SWD.SheritonHotel.Domain.Entities;
using SWD.SheritonHotel.Domain.Queries.AccountQuery;

namespace SWD.SheritonHotel.Handlers.Handlers.AccountHandler.QueriesHandler
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

            var certificatePath = user.CertificatePath;

            return new BaseResponse<ApplicationUser>
            {
                IsSucceed = true,
                Result = user,
                Message = "User profile fetched successfully"
            };
        }
    }
}
