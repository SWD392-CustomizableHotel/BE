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
    public class CancelPaymentCommandHandler : IRequestHandler<CancelPaymentCommand, ResponseDto<string>>
    {
        public async Task<ResponseDto<string>> Handle(CancelPaymentCommand request, CancellationToken cancellationToken)
        {
            var paymentService = new PaymentIntentService();
            var paymentIntent = paymentService.Get(request.PaymentIntentId);
            var invoiceId = paymentIntent.InvoiceId;
            var service = new InvoiceService();
            var invoice = service.SendInvoice(invoiceId);
            if (invoice == null)
            {
                return new ResponseDto<string>
                {
                    IsSucceeded = false,
                    Message = "Invoice is empty"
                };
            }
            return new ResponseDto<string>
            {
                IsSucceeded = true,
                Message = "Success"
            };
        }
    }
}
