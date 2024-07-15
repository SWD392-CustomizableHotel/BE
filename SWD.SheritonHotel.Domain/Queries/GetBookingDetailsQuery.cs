using MediatR;
using SWD.SheritonHotel.Domain.DTO;

namespace SWD.SheritonHotel.Domain.Queries;

public class GetBookingDetailsQuery : IRequest<BookingDetailsDto>
{
    public int BookingId { get; set; }

    public GetBookingDetailsQuery(int bookingId)
    {
        BookingId = bookingId;
    }
}