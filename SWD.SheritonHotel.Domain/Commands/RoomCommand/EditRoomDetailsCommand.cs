using MediatR;
using Microsoft.AspNetCore.Http;
using SWD.SheritonHotel.Domain.DTO.Responses;
using SWD.SheritonHotel.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Domain.Commands.RoomCommand
{
    public class EditRoomDetailsCommand : IRequest<ResponseDto<Room>>
    {
        public int RoomId { get; set; }
        public string RoomType { get; set; }
        public decimal RoomPrice { get; set; }
        public IFormFile ImageFile { get; set; }

        public EditRoomDetailsCommand(int roomId, string type, decimal price, IFormFile imageFile)
        {
            RoomId = roomId;
            RoomType = type;
            RoomPrice = price;
            ImageFile = imageFile;
        }
    }
}
