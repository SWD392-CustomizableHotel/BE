using Entities;
using MediatR;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.OtherObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Domain.Commands
{
    public class UpdateServiceStatusCommand : IRequest<ResponseDto<Service>>
    {
        [Required]
        public int ServiceId { get; set; }
        [Required]
        public ServiceStatus Status { get; set; }
    }
}
