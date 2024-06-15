using MediatR;
using SWD.SheritonHotel.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Domain.Queries
{
    public class GetRoomDetailsQuery : IRequest<ResponseDto<RoomDto>>
    {
        public int RoomId { get; set; }

        public GetRoomDetailsQuery(int roomId)
        {
            RoomId = roomId;
        }
    }
}
