using Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Domain.Queries
{
    public class GetUserByEmailQuery : IRequest<ApplicationUser>
    {
        public string Email {  get; set; }
    }
}
