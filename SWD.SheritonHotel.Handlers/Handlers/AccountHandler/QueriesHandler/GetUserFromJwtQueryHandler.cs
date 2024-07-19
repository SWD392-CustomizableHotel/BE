using MediatR;
using Microsoft.AspNetCore.Identity;
using SWD.SheritonHotel.Domain.DTO.Responses;
using SWD.SheritonHotel.Domain.Entities;
using SWD.SheritonHotel.Domain.Queries.AccountQuery;
using SWD.SheritonHotel.Validator.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Handlers.Handlers.AccountHandler.QueriesHandler
{
    public class GetUserFromJwtQueryHandler : IRequestHandler<GetUserFromJwtQuery, ResponseDto<ApplicationUser>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenValidator _tokenValidator;

        public GetUserFromJwtQueryHandler(UserManager<ApplicationUser> userManager, ITokenValidator tokenValidator)
        {
            _userManager = userManager;
            _tokenValidator = tokenValidator;
        }

        public async Task<ResponseDto<ApplicationUser>> Handle(GetUserFromJwtQuery request, CancellationToken cancellationToken)
        {
            var principal = _tokenValidator.ValidateToken(request.Jwt);

            if (principal == null)
            {
                return new ResponseDto<ApplicationUser>
                {
                    IsSucceeded = false,
                    Errors = new[] { "Invalid token" },
                    Data = null
                };
            }

            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return new ResponseDto<ApplicationUser>
                {
                    IsSucceeded = false,
                    Errors = new[] { "Invalid token: User identifier not found" },
                    Data = null
                };
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new ResponseDto<ApplicationUser>
                {
                    IsSucceeded = false,
                    Errors = new[] { "User not found" },
                    Data = null
                };
            }
            return new ResponseDto<ApplicationUser>
            {
                IsSucceeded = true,
                Message = "User details fetched successfully",
                Data = user,
            };
        }
    }
}
