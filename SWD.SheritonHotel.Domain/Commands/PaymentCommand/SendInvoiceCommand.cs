using MediatR;
using SWD.SheritonHotel.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Domain.Commands.PaymentCommand
{
    public class SendInvoiceCommand : IRequest<List<string>>
    {
        public string PaymentIntentId { get; set; }
    }
}
