using Microsoft.AspNetCore.Http;
using SWD.SheritonHotel.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Services.Interfaces
{
    public interface IIdentityCardService
    {
        Task<IdentityCardDto> UploadIdentityCardAsync(IFormFile frontFile, string userId, CancellationToken cancellationToken);
    }
}
