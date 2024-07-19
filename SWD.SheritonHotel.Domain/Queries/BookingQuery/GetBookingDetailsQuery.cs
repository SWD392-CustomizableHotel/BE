using MediatR;
using SWD.SheritonHotel.Domain.DTO.Booking;

namespace SWD.SheritonHotel.Domain.Queries.BookingQuery;

public class GetBookingDetailsQuery : IRequest<BookingDetailsDto>
{
    public int BookingId { get; set; }

    public GetBookingDetailsQuery(int bookingId)
    {
        BookingId = bookingId;
    }
}