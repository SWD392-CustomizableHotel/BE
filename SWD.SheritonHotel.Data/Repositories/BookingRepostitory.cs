using System.Linq.Dynamic.Core;
using AutoMapper;
using Entities;
using Microsoft.EntityFrameworkCore;
using SWD.SheritonHotel.Data.Base;
using SWD.SheritonHotel.Data.Context;
using SWD.SheritonHotel.Data.Repositories.Interfaces;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.OtherObjects;

namespace SWD.SheritonHotel.Data.Repositories;

public class BookingRepostitory : BaseRepository<Booking>, IBookingRepository
{

    private readonly ApplicationDbContext _context;
    public BookingRepostitory(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<(List<BookingHistoryDto>, int)> GetBookingHistoryAsync(string userId, int pageNumber, int pageSize, BookingFilter bookingFilter,
        string searchTerm)
    {
        var query = _context.Booking
            .Include(b => b.Room)
            .Include(b => b.Payments)
            .Include(b => b.BookingServices).ThenInclude(bs => bs.Service)
            .Include(b => b.BookingAmenities).ThenInclude(ba => ba.Amenity)
            .Where(b => b.UserId == userId)
            .AsQueryable();
        if (bookingFilter != null)
        {
            if (bookingFilter.RoomId.HasValue)
            {
                query = query.Where(b => b.RoomId == bookingFilter.RoomId.Value);
            }
            if (bookingFilter.Rating.HasValue)
            {
                query = query.Where(b => b.Rating == bookingFilter.Rating.Value);
            }
        }

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(b =>
                b.Room.Type.Contains(searchTerm) ||
                b.Room.Description.Contains(searchTerm) ||
                b.User.UserName.Contains(searchTerm) ||
                b.Rating.ToString().Contains(searchTerm)
            );
        }
        var totalRecords = await query.CountAsync();
        var bookings = await query
            .OrderBy(b => b.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(b => new BookingHistoryDto
            {
                BookingId = b.Id,
                RoomType = b.Room.Type,
                RoomDescription = b.Room.Description,
                Rating = b.Rating,
                UserName = b.User.UserName,
                StartDate = b.StartDate,
                EndDate = b.EndDate,
                Services = b.BookingServices.Select(bs => new ServiceDto
                {
                    Name = bs.Service.Name,
                    Code = bs.Service.Code,
                    Description = bs.Service.Description,
                    Price = bs.Service.Price
                }).ToList(),
                Amenities = b.BookingAmenities.Select(ba => new AmenityDTO
                {
                    Name = ba.Amenity.Name,
                    Code = ba.Amenity.Code,
                    Description = ba.Amenity.Description,
                    Price = ba.Amenity.Price
                }).ToList(),
                Payments = b.Payments.Select(p => new PaymentDto
                {
                    Amount = p.Amount,
                    Status = p.Status
                }).ToList()
            })
            .ToListAsync();
        return (bookings, totalRecords);
    }
    public async Task<int> CreateBookingAsync(Booking booking)
    {
        Add(booking);
        await _context.SaveChangesAsync();
        return booking.Id;
    }
}