using MediatR;
using SWD.SheritonHotel.Domain.Commands.PaymentCommand;
using SWD.SheritonHotel.Services.Interfaces;

namespace SWD.SheritonHotel.Handlers.Handlers.PaymentHandler.CommandsHandler;

public class PaymentCommandHandler : IRequestHandler<PaymentCommand, bool>
{
    private readonly IBookingService _bookingService;

    public PaymentCommandHandler(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }
    public async Task<bool> Handle(PaymentCommand request, CancellationToken cancellationToken)
    {
        return await _bookingService.Payment(request.BookingId, request.PaymentMethod);
    }
}