using MediatR;
using SWD.SheritonHotel.Data.Base;
using SWD.SheritonHotel.Domain.Commands.HotelCommand;
using SWD.SheritonHotel.Domain.Entities;
using SWD.SheritonHotel.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Handlers.Handlers.HotelHandler.CommandsHandler
{
    public class SeedsHotelCommandHandler : IRequestHandler<SeedsHotelCommand, List<Hotel>>
    {
        private readonly IHotelService _hotelService;
        public SeedsHotelCommandHandler(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }
        public async Task<List<Hotel>> Handle(SeedsHotelCommand request, CancellationToken cancellationToken)
        {
            _hotelService.SeedHotelsAsync(request.Hotel);

            return request.Hotel;
        }
    }
}
