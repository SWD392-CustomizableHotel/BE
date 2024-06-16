using Entities;
using MediatR;
using SWD.SheritonHotel.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Domain.Commands
{
    public class EditRoomDetailsCommand : IRequest<ResponseDto<Room>>
    {
        public int RoomId { get; set; }
        public string RoomType { get; set; }
        public decimal RoomPrice { get; set; }

        public EditRoomDetailsCommand(int roomId, string type, decimal price)
        {
            RoomId = roomId;
            RoomType = type;
            RoomPrice = price;
        }
    }
}
