using Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Domain.Commands
{
    public class GenerateResetPasswordCommand : IRequest<string>
    {
        public ApplicationUser User { get; set; }
    }
}
