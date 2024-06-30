using AutoMapper;
using Entities;
using Interfaces;
using MediatR;
using SWD.SheritonHotel.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Handlers.Handlers
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, ApplicationUser>
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public UpdateUserCommandHandler(IUserService userService, IMapper mapper) { 
            _userService = userService;
            _mapper = mapper;
        }
        public async Task<ApplicationUser> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<ApplicationUser>(request);
            _userService.UpdateAsync(user);
            return user;
        }
    }
}
