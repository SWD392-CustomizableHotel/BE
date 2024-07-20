using SWD.SheritonHotel.Domain.Base;
using SWD.SheritonHotel.Domain.DTO.Payment;
using SWD.SheritonHotel.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Data.Repositories.Interfaces
{
    public interface IPaymentRepository : IBaseRepository<Payment>
    {
        Task<int> CreatePaymentAsync(Payment payment);
        Task<PaymentDto> GetPaymentByBookingIdAsync(int bookingId);
        Task<Payment> GetPaymentByPaymentIntentIdAsync(string paymentIntentId);
        Task UpdatePaymentAsync(Payment payment);
    }
}
