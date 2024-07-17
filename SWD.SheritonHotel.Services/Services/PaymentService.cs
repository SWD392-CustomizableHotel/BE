using Entities;
using SWD.SheritonHotel.Data.Repositories.Interfaces;
using SWD.SheritonHotel.Domain.DTO;
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

        public async Task<PaymentDto> GetPaymentByBookingIdAsync(int bookingId)
        {
            return await _paymentRepository.GetPaymentByBookingIdAsync(bookingId);
        }

        public async Task<Payment> UpdatePaymentStatusAsync(string paymentIntentId, string status)
        {
            var payment = await _paymentRepository.GetPaymentByPaymentIntentIdAsync(paymentIntentId);
            if (payment != null)
            {
                payment.Status = status;
                await _paymentRepository.UpdatePaymentAsync(payment);
            }
            return payment;
        }
    }
}
