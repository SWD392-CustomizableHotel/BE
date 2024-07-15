using Stripe.Forwarding;
using Stripe;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWD.SheritonHotel.Domain.Commands;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace SWD.SheritonHotel.Services.Services
{
    public class PaymentIntentCustomizeService : IPaymentIntentCustomizeService
    {
        private readonly IRoomService _roomService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PaymentIntentCustomizeService(IRoomService roomService, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor) {
            _roomService = roomService;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<List<string>> CreatePaymentIntent(CreatePaymentIntentDTO request)
        {
            if (_httpContextAccessor.HttpContext == null || _httpContextAccessor.HttpContext.User == null)
            {
                throw new Exception("Needed login as Customer");
            }
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

            if (user == null)
            {
                throw new Exception("Needed login");
            }

            //Create Product
            var roomOptions = new ProductCreateOptions { Name = request.Items[0].roomId };
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
                Name = user.UserName,
                Email = user.Email,
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
                Expand = new List<string> { "payment_intent" }
            };
            var finalizeService = new InvoiceService();
            var finalizeInvoice = finalizeService.FinalizeInvoice(invoice.Id, finalizeOptions);

            //Client secret get
            var clientSecret = finalizeInvoice.PaymentIntent.ClientSecret;
            var invoiceId = invoice.Id;
            var list = new List<string> { clientSecret, invoiceId };
            //Return to FE
            return list;
        }
        private int CalculateOrderAmount(CreatePaymentIntentDTO.Item[] items)
        {
            return items.Sum(item => (item.roomPrice * item.numberOfDay * item.numberOfRoom * 100) + (item.amenityPrice * 100));
        }
    }
}
