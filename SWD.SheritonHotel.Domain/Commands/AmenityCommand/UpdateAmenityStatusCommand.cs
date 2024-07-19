using MediatR;
using SWD.SheritonHotel.Domain.DTO.Responses;
using SWD.SheritonHotel.Domain.Entities;
using SWD.SheritonHotel.Domain.OtherObjects;
using System.ComponentModel.DataAnnotations;

namespace SWD.SheritonHotel.Domain.Commands.AmenityCommand
{
    public class UpdateAmenityStatusCommand : IRequest<ResponseDto<Amenity>>
    {
        [Required]
        public int AmenityId { get; set; }
        [Required]
        public AmenityStatus Status { get; set; }
    }
}
