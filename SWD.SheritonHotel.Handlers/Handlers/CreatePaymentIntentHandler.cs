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
    public class CreatePaymentIntentHandler : IRequestHandler<CreatePaymentIntentCommand, string>
    {
        public Task<string> Handle(CreatePaymentIntentCommand request, CancellationToken cancellationToken)
        {
            var paymentIntentService = new PaymentIntentService();
            var paymentIntent = paymentIntentService.Create(new PaymentIntentCreateOptions
            {
                Amount = CalculateOrderAmount(request.Items),
                Currency = "usd",
                AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                {
                    Enabled = true,
                },
            });

            return Task.FromResult(paymentIntent.ClientSecret);
        }

        private int CalculateOrderAmount(CreatePaymentIntentCommand.Item[] items)
        {
            // Implement your order amount calculation logic here
            return items.Sum(item => item.Amount);
        }
    }
}
