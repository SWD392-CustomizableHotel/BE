﻿using MediatR;
using SWD.SheritonHotel.Domain.Commands.PaymentCommand;
using SWD.SheritonHotel.Services.Interfaces;

namespace SWD.SheritonHotel.Handlers.Handlers.BookingHandler.CommandsHandler;

public class CheckOutCommandHandler : IRequestHandler<CheckOutCommand, bool>
{
    private readonly IBookingService _bookingService;

    public CheckOutCommandHandler(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }
    public async Task<bool> Handle(CheckOutCommand request, CancellationToken cancellationToken)
    {
        return await _bookingService.CheckOut(request.BookingId);
    }
}