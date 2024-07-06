using MediatR;
using Microsoft.AspNetCore.Http;
using SWD.SheritonHotel.Domain.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Domain.Commands
{
    public class UploadIdentityCardCommand : IRequest<ResponseDto<IdentityCardDto>>
    {
        [Required]
        public IFormFile FrontFile { get; set; }
        public string UserId { get; set; }
    }
}
