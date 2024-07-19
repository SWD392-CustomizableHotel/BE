using Interfaces;
using MediatR;
using SWD.SheritonHotel.Domain.DTO.Responses;
using SWD.SheritonHotel.Domain.Entities;
using SWD.SheritonHotel.Domain.Queries.AccountQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Handlers.Handlers.AccountHandler.QueriesHandler
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, ResponseDto<ApplicationUser>>
    {
        protected readonly IUserService _userService;
        public GetUserByIdQueryHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<ResponseDto<ApplicationUser>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var details = await _userService.GetUserDetailsByIdAsync(request.UserId);
            return new ResponseDto<ApplicationUser>
            {
                Data = details,
                IsSucceeded = true,
                Message = "Fetch User details Successfully",
            };
        }
    }
}
