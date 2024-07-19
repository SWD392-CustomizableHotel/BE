using MediatR;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.Entities;
using SWD.SheritonHotel.Domain.Queries.RoomQuery;
using SWD.SheritonHotel.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Handlers.Handlers.RoomHandler.QueriesHandler
{
    public class GetAllCustomizableRoomsQueryHandler : IRequestHandler<GetAllCustomizableRoomsQuery, List<Room>>
    {
        private readonly IRoomService _roomService;
        public GetAllCustomizableRoomsQueryHandler(IRoomService roomService)
        {
            _roomService = roomService;
        }
        public async Task<List<Room>> Handle(GetAllCustomizableRoomsQuery request, CancellationToken cancellationToken)
        {
            return await _roomService.GetAllCustomizableRoomsAsync(cancellationToken, request.roomSize, request.numberOfPeople);
        }
    }
}
