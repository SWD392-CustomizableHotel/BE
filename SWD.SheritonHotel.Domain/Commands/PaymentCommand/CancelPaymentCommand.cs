using MediatR;
using SWD.SheritonHotel.Domain.DTO.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Domain.Commands.PaymentCommand
{
    public class CancelPaymentCommand : IRequest<ResponseDto<int>>
    {
        public string PaymentIntentId { get; set; }
    }
}
