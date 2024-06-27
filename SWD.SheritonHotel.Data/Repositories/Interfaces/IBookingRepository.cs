using Entities;
using SWD.SheritonHotel.Domain.Base;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.OtherObjects;

namespace SWD.SheritonHotel.Data.Repositories.Interfaces;

public interface IBookingRepository : IBaseRepository<Booking>
{
    Task<(List<BookingHistoryDto>, int)> GetBookingsByUserIdAsync(string userId, PaginationFilter paginationFilter);
}