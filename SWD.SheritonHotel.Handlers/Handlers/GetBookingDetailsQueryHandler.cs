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
    private readonly IMapper _mapper;

    public GetBookingDetailsQueryHandler(IBookingService bookingService, IMapper mapper)
    {
        _bookingService = bookingService;
        _mapper = mapper;
    }
    public async Task<BookingDetailsDto> Handle(GetBookingDetailsQuery request, CancellationToken cancellationToken)
    {
        var booking = await _bookingService.GetBookingDetails(request.BookingId);
        if (booking == null)
        {
            return null;
        }
        DateTime startDate = Convert.ToDateTime(booking.StartDate);
        DateTime endDate = Convert.ToDateTime(booking.EndDate);
        int time = (endDate - startDate).Days;
        var bookingDetails = _mapper.Map<BookingDetailsDto>(booking);
        var roomPrice = time * booking.Room.Price;
        var servicePrice = booking.BookingServices.Sum(bs => bs.Service.Price);
        var amenityPrice = booking.BookingAmenities.Sum(ba => ba.Amenity.Price);
        var paymentAmount = booking.Payments.Sum(p => p.Amount);
        bookingDetails.TotalPrice = paymentAmount;
        bookingDetails.UserName = booking.User.UserName;
        bookingDetails.BookingId = booking.Id;
        return bookingDetails;
    }
}