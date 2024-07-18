using MediatR;
using SWD.SheritonHotel.Domain.DTO.Responses;
using SWD.SheritonHotel.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Domain.Commands.PaymentCommand
{
    public class UpdatePaymentStatusCommand : IRequest<ResponseDto<Payment>>
    {
        public string PaymentIntentId { get; set; }
        public string Status { get; set; }
    }
}
