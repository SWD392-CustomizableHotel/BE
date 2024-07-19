using MediatR;
using SWD.SheritonHotel.Domain.DTO.Booking;
using SWD.SheritonHotel.Domain.DTO.Responses;
using SWD.SheritonHotel.Domain.OtherObjects;

namespace SWD.SheritonHotel.Domain.Queries.BookingQuery;

public class GetAllBookingHistoryByEndDateQuery : IRequest<PagedResponse<List<BookingHistoryDto>>>
{
    public PaginationFilter PaginationFilter { get; set; }
    public BookingFilter BookingFilter { get; set; }
    public string SearchTerm { get; set; }

    public GetAllBookingHistoryByEndDateQuery(PaginationFilter paginationFilter, BookingFilter bookingFilter, string searchTerm)
    {
        PaginationFilter = paginationFilter;
        BookingFilter = bookingFilter;
        SearchTerm = searchTerm;
    }
}