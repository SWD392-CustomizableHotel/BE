using Entities;
using SWD.SheritonHotel.Domain.Base;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.OtherObjects;

namespace SWD.SheritonHotel.Data.Repositories.Interfaces;

public interface IBookingRepository : IBaseRepository<Booking>
{
    Task<(List<BookingHistoryDto>, int)> GetBookingHistoryAsync(string userId, int pageNumber, int pageSize, BookingFilter bookingFilter, string searchTerm = null);
    Task<int> CreateBookingAsync(Booking booking);
    Task<(List<BookingHistoryDto>, int)> GetAllBookingHistoryByEndDateAsync(int pageNumber, int pageSize, BookingFilter bookingFilter, string searchTerm);
    Task<Booking> GetByIdAsync(int bookingId);
    Task<(List<BookingHistoryDto>, int)> GetBookingHistoryByEmailAsync(string email, int pageNumber, int pageSize, BookingFilter bookingFilter, string searchTerm);
}