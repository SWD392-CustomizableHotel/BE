using MediatR;
using SWD.SheritonHotel.Domain.Commands;
using SWD.SheritonHotel.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Handlers.Handlers
{
    public class UploadCanvasAndUpdateRoomStatusCommandHandler : IRequestHandler<UploadCanvasAndUpdateRoomStatusCommand, string>
    {
        private IBlobStorageService _blobStorageService;
        private IRoomService _roomService;
        public UploadCanvasAndUpdateRoomStatusCommandHandler(IBlobStorageService blobStorageService, IRoomService roomService) 
        { 
            _blobStorageService = blobStorageService; 
            _roomService = roomService; 
        }
        public async Task<string> Handle(UploadCanvasAndUpdateRoomStatusCommand request, CancellationToken cancellationToken)
        {
            var room = await _roomService.GetRoomByIdAsync(request.RoomId);
            if (room == null)
            {
                throw new Exception("Room not found");
            }

            using var stream = request.canvasImage.OpenReadStream();
            var blobUrl = await _blobStorageService.UploadFileAsync(stream, Guid.NewGuid().ToString(), request.canvasImage.ContentType);

            room.Status = "Booked";
            room.CanvasImage = blobUrl;

            _roomService.UpdateRoom(room);

            return room.Id.ToString();
        }
    }
}
