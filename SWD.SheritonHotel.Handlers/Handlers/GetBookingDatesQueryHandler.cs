using MediatR;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.Queries;
using SWD.SheritonHotel.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Handlers.Handlers
{
    public class GetBookingDatesQueryHandler : IRequestHandler<GetBookingDatesQuery, List<BookingDatesDto>>
    {
        private readonly IBookingService _bookingService;

        public GetBookingDatesQueryHandler(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        public async Task<List<BookingDatesDto>> Handle(GetBookingDatesQuery request, CancellationToken cancellationToken)
        {
            var bookings = await _bookingService.GetBookingDatesAsync(request.UserId);
            return bookings;
        }
    }
}
