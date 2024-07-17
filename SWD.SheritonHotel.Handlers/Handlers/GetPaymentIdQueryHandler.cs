using AutoMapper;
using MediatR;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.Queries;
using SWD.SheritonHotel.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Handlers.Handlers
{
    public class GetPaymentIdQueryHandler : IRequestHandler<GetPaymentIdQuery, ResponseDto<PaymentDto>>
    {
        private readonly IPaymentService _paymentService;

        public GetPaymentIdQueryHandler(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public async Task<ResponseDto<PaymentDto>> Handle(GetPaymentIdQuery request, CancellationToken cancellationToken)
        {
            var paymentDto = await _paymentService.GetPaymentByBookingIdAsync(request.BookingId);

            if (paymentDto == null)
            {
                return new ResponseDto<PaymentDto>
                {
                    IsSucceeded = false,
                    Message = "Payment not found",
                    Errors = new[] { "The specified payment could not be found." }
                };
            }

            return new ResponseDto<PaymentDto>
            {
                IsSucceeded = true,
                Message = "Payment retrieved successfully",
                Data = paymentDto
            };
        }
    }
}
