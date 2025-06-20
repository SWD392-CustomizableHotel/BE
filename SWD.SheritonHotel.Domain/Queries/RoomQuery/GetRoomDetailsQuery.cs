﻿using MediatR;
using SWD.SheritonHotel.Domain.DTO.Responses;
using SWD.SheritonHotel.Domain.DTO.Room;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Domain.Queries.RoomQuery
{
    public class GetRoomDetailsQuery : IRequest<ResponseDto<RoomDetailsDTO>>
    {
        public int RoomId { get; set; }

        public GetRoomDetailsQuery(int roomId)
        {
            RoomId = roomId;
        }
    }
}
