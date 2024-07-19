using MediatR;
using SWD.SheritonHotel.Domain.DTO.Amenity;
using SWD.SheritonHotel.Domain.DTO.Responses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Domain.Queries.AmenityQuery
{
    public class GetAmenitiesByRoomIdQuery : IRequest<ResponseDto<List<AmenityDTO>>>
    {
        [Required]
        public int RoomId { get; set; }

        public GetAmenitiesByRoomIdQuery(int roomId)
        {
            RoomId = roomId;
        }
    }
}
