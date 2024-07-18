using MediatR;
using SWD.SheritonHotel.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Domain.Queries.AccountQuery
{
    public class GetUserByEmailQuery : IRequest<ApplicationUser>
    {
        [EmailAddress]
        public string Email { get; set; }
    }
}
