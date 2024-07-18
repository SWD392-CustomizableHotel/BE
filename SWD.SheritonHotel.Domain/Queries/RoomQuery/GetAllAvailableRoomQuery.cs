using MediatR;
using SWD.SheritonHotel.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Domain.Queries.RoomQuery
{
    public class GetAllAvailableRoomQuery : IRequest<List<Room>>
    {
        public GetAllAvailableRoomQuery() { }
    }
}
