using Entities;
using MediatR;
using SWD.SheritonHotel.Domain.DTO;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace SWD.SheritonHotel.Domain.Commands
{
    public class CreateAmenityCommand : IRequest<ResponseDto<Amenity>>
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [DefaultValue("Normal")]
        public string Status { get; set; } = "Normal";
        [Required]
        public int HotelId { get; set; }
    }
}
