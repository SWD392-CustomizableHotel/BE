using MediatR;
using SWD.SheritonHotel.Domain.DTO.Responses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Domain.Commands.ServiceCommand
{
    public class DeleteServiceCommand : IRequest<ResponseDto<bool>>
    {
        [Required]
        public int ServiceId { get; set; }
    }
}
