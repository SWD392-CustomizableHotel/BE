using Entities;
using MediatR;
using SWD.SheritonHotel.Domain.DTO;

namespace SWD.SheritonHotel.Domain.Commands
{
    public class UpdateRoomStatusCommand : IRequest<ResponseDto<Room>>
    {
        public int RoomId { get; set; }
        public string RoomStatus { get; set; }
    }
}
