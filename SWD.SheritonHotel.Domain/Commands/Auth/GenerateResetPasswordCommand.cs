using MediatR;
using SWD.SheritonHotel.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Domain.Commands.Auth
{
    public class GenerateResetPasswordCommand : IRequest<string>
    {
        public ApplicationUser User { get; set; }
    }
}
