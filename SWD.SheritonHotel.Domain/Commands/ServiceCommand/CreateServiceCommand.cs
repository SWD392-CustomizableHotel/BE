using MediatR;
using SWD.SheritonHotel.Domain.DTO.Responses;
using SWD.SheritonHotel.Domain.Entities;
using SWD.SheritonHotel.Domain.OtherObjects;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SWD.SheritonHotel.Domain.Commands.ServiceCommand
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
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public int HotelId { get; set; }
    }
}
