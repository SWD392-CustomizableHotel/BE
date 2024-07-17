using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SWD.SheritonHotel.Data.Repositories.Interfaces;
using SWD.SheritonHotel.Domain.Commands;
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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PaymentService(IPaymentRepository paymentRepository, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _paymentRepository = paymentRepository;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
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

        public async Task<ResponseDto<int>> CreatePaymentForLaterAsync(CreatePaymentForLaterCommand request)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

            var newPayment = new Payment
            {
                Code = request.Code,
                Amount = request.Amount,
                BookingId = request.BookingId,
                PaymentIntentId = request.PaymentIntentId,
                Status = request.Status,
                CreatedBy = user.UserName,
                LastUpdatedBy = user.UserName,
                StartDate = request.StartDate.ToLocalTime(),
                EndDate = request.EndDate.ToLocalTime(),
                CreatedDate = DateTime.UtcNow.ToLocalTime(),
            };

            try
            {
                var newPaymentId = await CreatePaymentAsync(newPayment);
                return new ResponseDto<int>(newPaymentId);
            }
            catch (Exception ex)
            {
                return new ResponseDto<int>
                {
                    IsSucceeded = false,
                    Message = "An error occurred while creating the payment.",
                    Errors = new[] { ex.Message }
                };
            }
        }
    }
}
