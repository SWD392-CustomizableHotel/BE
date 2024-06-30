using AutoMapper;
using Entities;
using MediatR;
using SWD.SheritonHotel.Domain.Queries;
using SWD.SheritonHotel.Services.Interfaces;
using SWD.SheritonHotel.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Handlers.Handlers
{
    public class GetAllAvailableRoomQueryHandler : IRequestHandler<GetAllAvailableRoomQuery, List<Room>>
    {
        protected readonly IViewRoomService _viewRoomService;

        public GetAllAvailableRoomQueryHandler(IViewRoomService viewRoomService)
        {
            _viewRoomService = viewRoomService;
        }

        public async Task<List<Room>> Handle(GetAllAvailableRoomQuery request, CancellationToken cancellationToken)
        {
            return await _viewRoomService.GetAllAvailableRoom();
        }
    }
}
