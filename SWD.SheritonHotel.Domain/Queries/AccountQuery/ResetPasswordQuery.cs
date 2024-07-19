using MediatR;
using SWD.SheritonHotel.Domain.DTO.Responses;
using SWD.SheritonHotel.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Domain.Queries.AccountQuery
{
    public class ResetPasswordQuery : IRequest<BaseResponse<ApplicationUser>>
    {
        public string Password { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
