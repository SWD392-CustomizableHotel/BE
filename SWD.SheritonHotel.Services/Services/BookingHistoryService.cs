using Entities;
using SWD.SheritonHotel.Data.Repositories.Interfaces;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.OtherObjects;
using SWD.SheritonHotel.Services.Interfaces;

namespace SWD.SheritonHotel.Services.Services;

public class BookingHistoryService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IBookingAmenityRepository _bookingAmenityRepository;

    public BookingHistoryService(IBookingRepository bookingRepository, IBookingAmenityRepository bookingAmenityRepository)
    {
        _bookingRepository = bookingRepository;
        _bookingAmenityRepository = bookingAmenityRepository;
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
	public async Task<(List<CombinedBookingHistoryDto>, int)> GetAllBookingHistoryByStartDateAsync(int pageNumber, int pageSize, CombineBookingFilter combineBookingFilter, string searchTerm)
	{
        return await _bookingRepository.GetAllBookingHistoryByStartDateAsync(pageNumber, pageSize, combineBookingFilter, searchTerm);
    }

    public async Task<int> CreateBookingAmenityAsync(BookingAmenity bookingAmenity)
    {
        _bookingAmenityRepository.Add(bookingAmenity);
        return await _bookingAmenityRepository.SaveChangesAsync();
    }

    public async Task<Booking> GetBookingByIdAsync(int bookingId)
    {
        return await _bookingRepository.GetBookingByIdAsync(bookingId);
    }
}