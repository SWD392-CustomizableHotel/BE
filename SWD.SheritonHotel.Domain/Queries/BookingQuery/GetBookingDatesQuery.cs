using MediatR;
using SWD.SheritonHotel.Domain.DTO.Booking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Domain.Queries.BookingQuery
{
    public class GetBookingDatesQuery : IRequest<List<BookingDatesDto>>
    {
        public string UserId { get; set; }

        public GetBookingDatesQuery(string userId)
        {
            UserId = userId;
        }
    }
}
