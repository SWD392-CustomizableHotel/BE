using MediatR;
using SWD.SheritonHotel.Domain.DTO.Responses;
using SWD.SheritonHotel.Domain.Entities;

namespace SWD.SheritonHotel.Domain.Commands.RoomCommand
{
    public class UpdateRoomStatusCommand : IRequest<ResponseDto<Room>>
    {
        public int RoomId { get; set; }
        public string RoomStatus { get; set; }
    }
}
