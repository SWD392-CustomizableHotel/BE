using Entities;
using MediatR;
using SWD.SheritonHotel.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Domain.Queries
{
    public class GetUserFromJwtQuery : IRequest<ResponseDto<ApplicationUser>>
    {
        public string Jwt { get; set; }

        public GetUserFromJwtQuery(string jwt)
        {
            Jwt = jwt;
        }
    }
}
