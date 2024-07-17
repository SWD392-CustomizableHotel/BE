using AutoMapper;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Stripe;
using SWD.SheritonHotel.Data.Repositories.Interfaces;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.OtherObjects;
using SWD.SheritonHotel.Services.Interfaces;

namespace SWD.SheritonHotel.Services.Services;

public class BookingHistoryService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IBookingAmenityRepository _bookingAmenityRepository;
    private readonly IAmentiyRepository _amentiyRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IMapper _mapper;
    private readonly IPaymentRepository _paymentRepository;

    public BookingHistoryService(IBookingRepository bookingRepository, IBookingAmenityRepository bookingAmenityRepository, IAmentiyRepository amentiyRepository, IRoomRepository roomRepository, IMapper mapper, IPaymentRepository paymentRepository)
    {
        _bookingRepository = bookingRepository;
        _bookingAmenityRepository = bookingAmenityRepository;
        _amentiyRepository = amentiyRepository;
        _roomRepository = roomRepository;
        _mapper = mapper;
        _paymentRepository = paymentRepository;
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

    public async Task<(List<BookingHistoryDto>, int)> GetAllBookingHistoryByEndDateAsync(int pageNumber, int pageSize, BookingFilter bookingFilter, string searchTerm)
    {
        return await _bookingRepository.GetAllBookingHistoryByEndDateAsync(pageNumber, pageSize, bookingFilter,
            searchTerm);
    }

    public async Task<BookingDetailsDto> GetBookingDetailsDto(int bookingId)
    {
        var booking = await _bookingRepository.GetByIdAsync(bookingId);
        if (booking == null)
        {
            return null;
        }
        DateTime startDate = Convert.ToDateTime(booking.StartDate);
        DateTime endDate = Convert.ToDateTime(booking.EndDate);
        int time = (endDate - startDate).Days;

        var bookingDetails = _mapper.Map<BookingDetailsDto>(booking);
        var roomPrice = time * booking.Room.Price;
        var amenityPrice = booking.BookingAmenities.Sum(ba => ba.Amenity.Price);
        var paymentAmount = booking.Payments.Sum(p => p.Amount);

        bookingDetails.TotalPrice = paymentAmount;
        bookingDetails.UserName = booking.User.UserName;
        bookingDetails.BookingId = booking.Id;

        return bookingDetails;
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

    public async Task<bool> CheckOut(int bookingId)
    {
        var booking = await _bookingRepository.GetByIdAsync(bookingId);
        if (booking == null)
        {
            return false;
        }
        booking.IsDeleted = true;
        var room = booking.Room;
        room.Status = "Available";
        room.CanvasImage = null;
        _roomRepository.Update(room);
        foreach (var bookingAmenity in booking.BookingAmenities)
        {
            var amenity = await _amentiyRepository.GetAmenityByIdAsync(bookingAmenity.AmenityId);
            amenity.InUse -= 1;
            _amentiyRepository.Update(amenity);
        }
        return true;
    }

    public async Task<bool> Payment(int bookingId, string paymentMethod)
    {
        var booking = await _bookingRepository.GetByIdAsync(bookingId);
        if (booking == null)
        {
            return false;
        }

        foreach (var payments in booking.Payments)
        {
            var payment = await _paymentRepository.GetById(payments.Id);
            payments.Status = "Success";
            payment.PaymentMethod = paymentMethod;
            _paymentRepository.UpdatePayment(payment);
        }

        return true;
    }

    public async Task<(List<BookingHistoryDto>, int)> GetBookingHistoryByEmailAsync(string email, int pageNumber, int pageSize, BookingFilter bookingFilter,
        string searchTerm = null)
    {
        return await _bookingRepository.GetBookingHistoryByEmailAsync(email, pageNumber, pageSize, bookingFilter, searchTerm);
    }
}