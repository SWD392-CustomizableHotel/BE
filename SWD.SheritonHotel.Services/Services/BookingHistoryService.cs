using Entities;
using SWD.SheritonHotel.Data.Repositories.Interfaces;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.OtherObjects;
using SWD.SheritonHotel.Services.Interfaces;

namespace SWD.SheritonHotel.Services.Services;

public class BookingHistoryService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;

    public BookingHistoryService(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }
    
    public async Task<(List<BookingHistoryDto>, int)> GetBookingHistoryAsync(string userId, int pageNumber, int pageSize, BookingFilter bookingFilter,
        string searchTerm = null)
    {
        return await _bookingRepository.GetBookingHistoryAsync(userId, pageNumber, pageSize, bookingFilter, searchTerm);
    }
    public async Task<int> CreateBookingAsync(Booking booking)
    {
        return await _bookingRepository.CreateBookingAsync(booking);
    }
    public async Task<List<BookingDatesDto>> GetBookingDatesAsync(string userId)
    {
        return await _bookingRepository.GetBookingDatesAsync(userId);
    }
}