using MediatR;
using SWD.SheritonHotel.Domain.DTO.Booking;
using SWD.SheritonHotel.Domain.Queries.BookingQuery;
using SWD.SheritonHotel.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Handlers.Handlers.BookingHandler.QueriesHandler
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
