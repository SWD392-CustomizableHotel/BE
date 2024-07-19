using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SWD.SheritonHotel.Domain.Commands.IdentityCard;
using SWD.SheritonHotel.Domain.DTO.IdentityCard;
using SWD.SheritonHotel.Domain.DTO.Responses;
using SWD.SheritonHotel.Domain.Entities;
using SWD.SheritonHotel.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Handlers.Handlers.IdentityCardHandler.CommandsHandler
{
    public class UploadIdentityCardCommandHandler : IRequestHandler<UploadIdentityCardCommand, ResponseDto<IdentityCardDto>>
    {
        private readonly IIdentityCardService _identityCardService;

        public UploadIdentityCardCommandHandler(IIdentityCardService identityCardService)
        {
            _identityCardService = identityCardService;
        }

        public async Task<ResponseDto<IdentityCardDto>> Handle(UploadIdentityCardCommand request, CancellationToken cancellationToken)
        {
            return await _identityCardService.UploadIdentityCardWithValidationAsync(request, cancellationToken);
        }
    }
}
