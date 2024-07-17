using MediatR;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.Queries;
using SWD.SheritonHotel.Services.Interfaces;

namespace SWD.SheritonHotel.Handlers.Handlers;

public class GetBookingHistoryByEmailQueryHandler : IRequestHandler<GetBookingHistoryByEmailQuery, PagedResponse<List<BookingHistoryDto>>>
{
    private readonly IBookingService _bookingService;

    public GetBookingHistoryByEmailQueryHandler(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    public async Task<PagedResponse<List<BookingHistoryDto>>> Handle(GetBookingHistoryByEmailQuery request, CancellationToken cancellationToken)
    {
        var validFilter = request.PaginationFilter;
        var (bookings, totalRecords) = await _bookingService.GetBookingHistoryByEmailAsync(request.Email, validFilter.PageNumber, validFilter.PageSize, request.BookingFilter, request.SearchTerm);
        var totalPages = (int)Math.Ceiling(totalRecords / (double)validFilter.PageSize);
        var response = new PagedResponse<List<BookingHistoryDto>>(bookings, validFilter.PageNumber, validFilter.PageSize)
        {
            TotalRecords = totalRecords,
            TotalPages = totalPages,
        };
        return response;
    }
}