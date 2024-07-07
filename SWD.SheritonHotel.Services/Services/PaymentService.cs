using Entities;
using SWD.SheritonHotel.Data.Repositories.Interfaces;
using SWD.SheritonHotel.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Services.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<int> CreatePaymentAsync(Payment payment)
        {
            return await _paymentRepository.CreatePaymentAsync(payment);
        }
    }
}
