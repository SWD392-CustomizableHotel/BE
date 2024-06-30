using MediatR;
using SWD.SheritonHotel.Domain.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Domain.Queries
{
    public class GetServicesByRoomIdQuery : IRequest<ResponseDto<List<ServiceDto>>>
    {
        [Required]
        public int RoomId { get; set; }

        public GetServicesByRoomIdQuery(int roomId)
        {
            RoomId = roomId;
        }
    }
}
