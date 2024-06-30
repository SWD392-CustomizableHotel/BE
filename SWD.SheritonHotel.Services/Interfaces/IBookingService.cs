using Entities;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.OtherObjects;

namespace SWD.SheritonHotel.Services.Interfaces;

public interface IBookingService
{
    Task<(List<BookingHistoryDto>, int)> GetBookingHistoryAsync(string userId, int pageNumber, int pageSize, BookingFilter bookingFilter, string searchTerm = null);
    Task<int> CreateBookingAsync(Booking booking);
}


