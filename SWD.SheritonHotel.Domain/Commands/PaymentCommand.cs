using MediatR;

namespace SWD.SheritonHotel.Domain.Commands;

public class PaymentCommand : IRequest<bool>
{
    public int BookingId { get; set; }
    public string PaymentMethod { get; set; }

    public PaymentCommand(int bookingId, string paymentMethod)
    {
        BookingId = bookingId;
        PaymentMethod = paymentMethod;
    }
}