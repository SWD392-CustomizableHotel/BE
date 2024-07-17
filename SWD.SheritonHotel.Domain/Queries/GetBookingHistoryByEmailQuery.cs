using MediatR;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.OtherObjects;

namespace SWD.SheritonHotel.Domain.Queries;

public class GetBookingHistoryByEmailQuery : IRequest<PagedResponse<List<BookingHistoryDto>>>
{
    public string Email { get; set; }
    public PaginationFilter PaginationFilter { get; set; }
    public BookingFilter BookingFilter { get; set; }
    public string SearchTerm { get; set; }

    public GetBookingHistoryByEmailQuery(string email, PaginationFilter paginationFilter, BookingFilter bookingFilter, string searchTerm)
    {
        Email = email;
        PaginationFilter = paginationFilter;
        BookingFilter = bookingFilter;
        SearchTerm = searchTerm;
    }
}