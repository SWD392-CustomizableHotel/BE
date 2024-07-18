using MediatR;
using SWD.SheritonHotel.Domain.DTO.Booking;
using SWD.SheritonHotel.Domain.DTO.Responses;
using SWD.SheritonHotel.Domain.OtherObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Domain.Queries.BookingQuery
{
    public class GetAllBookingHistoryByStartDateQuery : IRequest<PagedResponse<List<CombinedBookingHistoryDto>>>
    {
        public PaginationFilter PaginationFilter { get; set; }
        public CombineBookingFilter CombineBookingFilter { get; set; }
        public string SearchTerm { get; set; }

        public GetAllBookingHistoryByStartDateQuery(PaginationFilter paginationFilter, CombineBookingFilter combineBookingFilter, string searchTerm)
        {
            PaginationFilter = paginationFilter;
            CombineBookingFilter = combineBookingFilter;
            SearchTerm = searchTerm;
        }
    }
}
