using SWD.SheritonHotel.Domain.Commands.PaymentCommand;
using SWD.SheritonHotel.Domain.DTO.Payment;
using SWD.SheritonHotel.Domain.DTO.Responses;
using SWD.SheritonHotel.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<int> CreatePaymentAsync(Payment payment);
        Task<PaymentDto> GetPaymentByBookingIdAsync(int bookingId);
        Task<Payment> UpdatePaymentStatusAsync(string paymentIntentId, string status);
        Task<ResponseDto<int>> CreatePaymentForLaterAsync(CreatePaymentForLaterCommand request);
    }
}
