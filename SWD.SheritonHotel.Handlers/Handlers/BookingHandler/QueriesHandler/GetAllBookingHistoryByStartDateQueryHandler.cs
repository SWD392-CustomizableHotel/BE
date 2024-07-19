using MediatR;
using SWD.SheritonHotel.Domain.DTO.Booking;
using SWD.SheritonHotel.Domain.DTO.Responses;
using SWD.SheritonHotel.Domain.Queries.BookingQuery;
using SWD.SheritonHotel.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Handlers.Handlers.BookingHandler.QueriesHandler
{
    public class GetAllBookingHistoryByStartDateQueryHandler : IRequestHandler<GetAllBookingHistoryByStartDateQuery, PagedResponse<List<CombinedBookingHistoryDto>>>
    {
        private readonly IBookingService _bookingService;

        public GetAllBookingHistoryByStartDateQueryHandler(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        public async Task<PagedResponse<List<CombinedBookingHistoryDto>>> Handle(GetAllBookingHistoryByStartDateQuery request, CancellationToken cancellationToken)
        {
            var validFilter = request.PaginationFilter;

            var (bookings, totalRecords) = await _bookingService.GetAllBookingHistoryByStartDateAsync(validFilter.PageNumber, validFilter.PageSize, request.CombineBookingFilter, request.SearchTerm);

            var totalPages = (int)Math.Ceiling(totalRecords / (double)validFilter.PageSize);

            var response = new PagedResponse<List<CombinedBookingHistoryDto>>(bookings, validFilter.PageNumber, validFilter.PageSize)
            {
                TotalRecords = totalRecords,
                TotalPages = totalPages,
            };
            return response;
        }
    }
}
