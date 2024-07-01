using MediatR;
using SWD.SheritonHotel.Domain.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Domain.Commands
{
    public class DeleteAmenityCommand : IRequest<ResponseDto<bool>>
    {
        [Required]
        public int AmenityId { get; set; }  
    }
}
