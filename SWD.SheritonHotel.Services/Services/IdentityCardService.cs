using Microsoft.AspNetCore.Http;
using SWD.SheritonHotel.Data.Repositories.Interfaces;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.Entities;
using SWD.SheritonHotel.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Services.Services
{
    public class IdentityCardService : IIdentityCardService
    {
        private readonly IIdentityCardRepository _identityCardRepository;

        public IdentityCardService(IIdentityCardRepository identityCardRepository)
        {
            _identityCardRepository = identityCardRepository;
        }

        public async Task<IdentityCardDto> UploadIdentityCardAsync(IFormFile frontFile, int paymentId, CancellationToken cancellationToken)
        {
            return await _identityCardRepository.UploadIdentityCardAsync(frontFile, paymentId, cancellationToken);
        }

        public async Task<List<IdentityCardDto>> GetAllIdentityCardsAsync(CancellationToken cancellationToken)
        {
            return await _identityCardRepository.GetAllIdentityCardsAsync(cancellationToken);
        }
    }
}
