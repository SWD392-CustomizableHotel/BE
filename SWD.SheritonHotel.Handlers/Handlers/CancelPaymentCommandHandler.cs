using MediatR;
using Stripe;
using SWD.SheritonHotel.Domain.Commands;
using SWD.SheritonHotel.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Handlers.Handlers
{
    public class CancelPaymentCommandHandler : IRequestHandler<CancelPaymentCommand, ResponseDto<int>>
    {
        public async Task<ResponseDto<int>> Handle(CancelPaymentCommand request, CancellationToken cancellationToken)
        {
            var service = new InvoiceService();
            service.VoidInvoice(request.PaymentIntentId);
            return new ResponseDto<int>
            {
                IsSucceeded = true,
                Message = "Success"
            };
        }
    }
}
