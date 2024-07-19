using MediatR;
using SWD.SheritonHotel.Domain.DTO.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Domain.Commands.RoomCommand
{
    public class DeleteRoomCommand : IRequest<ResponseDto<bool>>
    {
        public int RoomId { get; set; }
    }
}
