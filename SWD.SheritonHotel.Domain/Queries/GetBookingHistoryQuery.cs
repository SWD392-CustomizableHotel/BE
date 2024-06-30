using MediatR;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.OtherObjects;

namespace SWD.SheritonHotel.Domain.Queries;

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