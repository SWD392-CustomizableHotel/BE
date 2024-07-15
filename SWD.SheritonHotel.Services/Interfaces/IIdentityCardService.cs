using Microsoft.AspNetCore.Http;
using SWD.SheritonHotel.Domain.DTO;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Services.Interfaces
{
    public interface IIdentityCardService
    {
        Task<IdentityCardDto> UploadIdentityCardAsync(IFormFile frontFile, int paymentId, CancellationToken cancellationToken);
        Task<List<IdentityCardDto>> GetAllIdentityCardsAsync(CancellationToken cancellationToken);
    }
}
