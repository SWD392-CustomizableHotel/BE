using MediatR;

namespace SWD.SheritonHotel.Domain.Commands;

public class CheckOutCommand : IRequest<bool>
{
    public int BookingId { get; set; }

    public CheckOutCommand(int bookingId)
    {
        BookingId = bookingId;
    }
}