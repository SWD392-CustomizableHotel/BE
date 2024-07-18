using Microsoft.AspNetCore.Http;
using SWD.SheritonHotel.Domain.Base;
using SWD.SheritonHotel.Domain.DTO.IdentityCard;
using SWD.SheritonHotel.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Data.Repositories.Interfaces
{
    public interface IIdentityCardRepository
    {
        Task<IdentityCardDto> UploadIdentityCardAsync(IFormFile frontFile, int paymentId, CancellationToken cancellationToken);
        Task<List<IdentityCardDto>> GetAllIdentityCardsAsync(CancellationToken cancellationToken);
    }
}
