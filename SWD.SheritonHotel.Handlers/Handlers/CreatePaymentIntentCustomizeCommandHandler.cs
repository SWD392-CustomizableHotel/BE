using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using SWD.SheritonHotel.Domain.Commands;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Handlers.Handlers
{
    public class CreatePaymentIntentCustomizeCommandHandler : IRequestHandler<CreatePaymentIntentCustomizableCommand, List<string>>
    {
        private readonly IPaymentIntentCustomizeService _service;
        private readonly IMapper _mapper;
        public CreatePaymentIntentCustomizeCommandHandler(IPaymentIntentCustomizeService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }
        public async Task<List<string>> Handle(CreatePaymentIntentCustomizableCommand request, CancellationToken cancellationToken)
        {
            var customizeRequest = _mapper.Map<CreatePaymentIntentDTO>(request);
            return _service.CreatePaymentIntent(customizeRequest);
        }
    }
}
