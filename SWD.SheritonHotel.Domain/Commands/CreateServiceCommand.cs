using Entities;
using MediatR;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.OtherObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Domain.Commands
{
    public class CreateServiceCommand : IRequest<ResponseDto<Service>>
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string Description { get; set; }
        [DefaultValue("Closed")]
        public ServiceStatus Status { get; set; } = ServiceStatus.Closed;
        [Required]
        public int HotelId { get; set; }
    }
}
