using Interfaces;
using MediatR;
using Services;
using SWD.SheritonHotel.Domain.Entities;
using SWD.SheritonHotel.Domain.Queries.AccountQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Handlers.Handlers.AccountHandler.QueriesHandler
{
    public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, ApplicationUser>
    {
        protected readonly IUserService _userService;
        public GetUserByEmailQueryHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<ApplicationUser> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            return await _userService.FindUserByEmail(request.Email);
        }
    }
}
