using Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SWD.SheritonHotel.Domain.Commands;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Services.Interfaces;
using SWD.SheritonHotel.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Handlers.Handlers
{
    public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, ResponseDto<int>>
    {
        private readonly IPaymentService _paymentService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreatePaymentCommandHandler(IPaymentService paymentService, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _paymentService = paymentService;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseDto<int>> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            if (user == null || !(await _userManager.IsInRoleAsync(user, "CUSTOMER")))
            {
                return new ResponseDto<int>
                {
                    IsSucceeded = false,
                    Message = "Unauthorized",
                    Errors = new[] { "You must be an customer to perform this operation." }
                };
            }
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
                var newPaymentId = await _paymentService.CreatePaymentAsync(newPayment);
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
