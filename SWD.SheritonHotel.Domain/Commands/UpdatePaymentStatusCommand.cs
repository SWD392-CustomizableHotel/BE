using Entities;
using MediatR;
using SWD.SheritonHotel.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Domain.Commands
{
    public class UpdatePaymentStatusCommand : IRequest<ResponseDto<Payment>>
    {
        public string PaymentIntentId { get; set; }
        public string Status { get; set; }
    }
}
