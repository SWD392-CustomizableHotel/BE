using Entities;
using MediatR;
using Stripe;
using SWD.SheritonHotel.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Handlers.Handlers
{
    public class CreatePaymentIntentHandler : IRequestHandler<CreatePaymentIntentCommand, List<string>>
    {
        public Task<List<string>> Handle(CreatePaymentIntentCommand request, CancellationToken cancellationToken)
        {
            /*var paymentIntentService = new PaymentIntentService();
            var paymentIntent = paymentIntentService.Create(new PaymentIntentCreateOptions
            {
                Amount = CalculateOrderAmount(request.Items),
                Currency = "VND",
                AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                {
                    Enabled = true,
                },
            });*/

            //Create Product
            var roomOptions = new ProductCreateOptions { Name = request.Items[0].RoomId };
            var roomService = new ProductService();
            var room = roomService.Create(roomOptions);

            //Create price
            var priceOptions = new PriceCreateOptions
            {
                Product = room.Id,
                UnitAmount = CalculateOrderAmount(request.Items),
                Currency = "usd"
            };
            var priceService = new PriceService();
            var price = priceService.Create(priceOptions);

            //Create a customer
            var customerOptions = new CustomerCreateOptions
            {
                Name = "Kiet",
                Email = request.Items[0].UserEmail,
                Description = "A special customer"
            };
            var customerService = new CustomerService();
            var customer = customerService.Create(customerOptions);

            //Create an invoice
            var invoiceOptions = new InvoiceCreateOptions
            {
                Customer = customer.Id,
                CollectionMethod = "charge_automatically",

            };
            var invoiceService = new InvoiceService();
            var invoice = invoiceService.Create(invoiceOptions);

            //Create an invoice item
            var invoiceItemOptions = new InvoiceItemCreateOptions
            {
                Customer = customer.Id,
                Price = price.Id,
                Invoice = invoice.Id
            };
            var invoiceItemService = new InvoiceItemService();
            var invoiceItem = invoiceItemService.Create(invoiceItemOptions);

            //Finalize invoice
            var finalizeOptions = new InvoiceFinalizeOptions
            {
                Expand = new List<string> { "payment_intent"}
            };
            var finalizeService = new InvoiceService();
            var finalizeInvoice = finalizeService.FinalizeInvoice(invoice.Id, finalizeOptions);

            //Client secret get
            var clientSecret = finalizeInvoice.PaymentIntent.ClientSecret;
            var invoiceId = invoice.Id;
            var list = new List<string>{clientSecret, invoiceId };
            //Return to FE
            
            return Task.FromResult(list);
        }

        private int CalculateOrderAmount(CreatePaymentIntentCommand.Item[] items)
        {
            // Implement your order amount calculation logic here
            return items.Sum(item => (item.RoomPrice * item.NumberOfDate * item.NumberOfRoom * 100));
        }
    }
}
