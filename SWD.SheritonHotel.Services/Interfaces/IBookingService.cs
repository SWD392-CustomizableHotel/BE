using Entities;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.OtherObjects;

namespace SWD.SheritonHotel.Services.Interfaces;

public interface IBookingService
{
    Task<(List<BookingHistoryDto>, int)> GetBookingHistoryAsync(string userId, int pageNumber, int pageSize, BookingFilter bookingFilter, string searchTerm = null);
    Task<int> CreateBookingAsync(Booking booking);
    Task<List<BookingDatesDto>> GetBookingDatesAsync(string userId);
    Task<(List<CombinedBookingHistoryDto>, int)> GetAllBookingHistoryByStartDateAsync(int pageNumber, int pageSize, CombineBookingFilter combineBookingFilter, string searchTerm);

    Task<(List<BookingHistoryDto>, int)> GetAllBookingHistoryByEndDateAsync(int pageNumber, int pageSize,
        BookingFilter bookingFilter, string searchTerm);
    Task<BookingDetailsDto> GetBookingDetailsDto(int bookingId);
    Task<int> CreateBookingAmenityAsync(BookingAmenity bookingAmenity);
    Task<Booking> GetBookingByIdAsync(int bookingId);
    Task<bool> CheckOut(int bookingId);
    Task<bool> Payment(int bookingId, string paymentMethod);

    Task<(List<BookingHistoryDto>, int)> GetBookingHistoryByEmailAsync(string email, int pageNumber, int pageSize,
        BookingFilter bookingFilter, string searchTerm = null);
}

