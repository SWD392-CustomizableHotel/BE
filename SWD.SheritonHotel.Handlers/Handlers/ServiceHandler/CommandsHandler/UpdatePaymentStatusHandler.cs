using MediatR;
using SWD.SheritonHotel.Domain.Commands.PaymentCommand;
using SWD.SheritonHotel.Domain.DTO.Responses;
using SWD.SheritonHotel.Domain.Entities;
using SWD.SheritonHotel.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Handlers.Handlers.ServiceHandler.CommandsHandler
{
    public class UpdatePaymentStatusHandler : IRequestHandler<UpdatePaymentStatusCommand, ResponseDto<Payment>>
    {
        private readonly IPaymentService _paymentService;

        public UpdatePaymentStatusHandler(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public async Task<ResponseDto<Payment>> Handle(UpdatePaymentStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var payment = await _paymentService.UpdatePaymentStatusAsync(request.PaymentIntentId, request.Status);
                if (payment == null)
                {
                    return new ResponseDto<Payment>
                    {
                        IsSucceeded = false,
                        Message = "Payment not found",
                        Errors = new[] { "Payment not found with provided PaymentIntentId" }
                    };
                }
                return new ResponseDto<Payment>(payment);
            }
            catch (Exception ex)
            {
                return new ResponseDto<Payment>
                {
                    IsSucceeded = false,
                    Message = "Failed to update payment status",
                    Errors = new[] { ex.Message }
                };
            }
        }
    }
}
