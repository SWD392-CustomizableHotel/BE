using Google.Apis.Auth.OAuth2;
using MediatR;
using Microsoft.AspNetCore.SignalR.Protocol;
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
    public class SendInvoiceCommandHandler : IRequestHandler<SendInvoiceCommand, List<string>>
    {
        public Task<List<string>> Handle(SendInvoiceCommand request, CancellationToken cancellationToken)
        {
            var service = new PaymentIntentService();
            var paymentIntent = service.Get(request.PaymentIntentId);
            var invoiceService = new InvoiceService();
            var invoice = invoiceService.Get(paymentIntent.InvoiceId);
            var invoiceDownloadLink = invoice.InvoicePdf;
            var invoiceHostedPages = invoice.HostedInvoiceUrl;
            var list = new List<string> { invoiceDownloadLink, invoiceHostedPages };

            return Task.FromResult(list);
        }
    }
}
