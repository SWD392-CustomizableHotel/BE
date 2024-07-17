using MediatR;
using SWD.SheritonHotel.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Domain.Queries
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
