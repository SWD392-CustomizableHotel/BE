using MediatR;
using SWD.SheritonHotel.Domain.DTO.Booking;
using SWD.SheritonHotel.Domain.DTO.Responses;
using SWD.SheritonHotel.Domain.OtherObjects;

namespace SWD.SheritonHotel.Domain.Queries.BookingQuery;

public class GetBookingHistoryQuery : IRequest<PagedResponse<List<BookingHistoryDto>>>
{
    public string UserId { get; set; }
    public PaginationFilter PaginationFilter { get; set; }
    public BookingFilter BookingFilter { get; set; }
    public string SearchTerm { get; set; }

    public GetBookingHistoryQuery(string userId, PaginationFilter paginationFilter, BookingFilter bookingFilter, string searchTerm)
    {
        UserId = userId;
        PaginationFilter = paginationFilter;
        BookingFilter = bookingFilter;
        SearchTerm = searchTerm;
    }
}