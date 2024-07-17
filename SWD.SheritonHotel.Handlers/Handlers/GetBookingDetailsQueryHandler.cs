using System.Diagnostics;
using AutoMapper;
using MediatR;
using SWD.SheritonHotel.Data.Repositories.Interfaces;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.Queries;
using SWD.SheritonHotel.Services.Interfaces;

namespace SWD.SheritonHotel.Handlers.Handlers;

public class GetBookingDetailsQueryHandler : IRequestHandler<GetBookingDetailsQuery, BookingDetailsDto>
{
    private readonly IBookingService _bookingService;
    public GetBookingDetailsQueryHandler(IBookingService bookingService)
    {
        _bookingService = bookingService;
        
    }
    public async Task<BookingDetailsDto> Handle(GetBookingDetailsQuery request, CancellationToken cancellationToken)
    {
        return await _bookingService.GetBookingDetailsDto(request.BookingId);
    }
}