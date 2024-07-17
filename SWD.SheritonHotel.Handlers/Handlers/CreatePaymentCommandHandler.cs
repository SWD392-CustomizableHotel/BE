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
    public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentForLaterCommand, ResponseDto<int>>
    {
        private readonly IPaymentService _paymentService;

        public CreatePaymentCommandHandler(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public async Task<ResponseDto<int>> Handle(CreatePaymentForLaterCommand request, CancellationToken cancellationToken)
        {
            return await _paymentService.CreatePaymentForLaterAsync(request);
        }
    }
}