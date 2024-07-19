using MediatR;
using SWD.SheritonHotel.Domain.DTO.Responses;
using SWD.SheritonHotel.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace SWD.SheritonHotel.Domain.Commands.AmenityCommand
{
    public class UpdateAmenityCommand : IRequest<ResponseDto<Amenity>>
    {
        [Required]
        public int AmenityId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int Capacity { get; set; }
        [Required]
        public int InUse { get; set; }
        [Required]
        public decimal Price { get; set; }
    }
}
