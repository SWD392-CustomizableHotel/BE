using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Domain.Commands.RoomCommand
{
    public class UploadCanvasAndUpdateRoomStatusCommand : IRequest<string>
    {
        public int RoomId { get; set; }
        public IFormFile canvasImage { get; set; }
    }
}
