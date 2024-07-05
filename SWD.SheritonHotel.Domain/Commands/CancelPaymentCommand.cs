using MediatR;
using SWD.SheritonHotel.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Domain.Commands
{
    public class CancelPaymentCommand : IRequest<ResponseDto<int>>
    {
        public string PaymentIntentId { get; set; }
    }
}
