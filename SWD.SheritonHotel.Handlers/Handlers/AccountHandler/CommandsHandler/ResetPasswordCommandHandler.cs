using Interfaces;
using MediatR;
using SWD.SheritonHotel.Domain.Commands.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Handlers.Handlers.AccountHandler.CommandsHandler
{
    public class ResetPasswordCommandHandler : IRequestHandler<GenerateResetPasswordCommand, string>
    {
        private readonly IUserService _userService;
        public ResetPasswordCommandHandler(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<string> Handle(GenerateResetPasswordCommand request, CancellationToken cancellationToken)
        {
            return await _userService.GenResetPasswordTokenAsync(request.User);
        }
    }
}
