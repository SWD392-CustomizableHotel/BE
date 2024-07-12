using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Domain.Commands
{
    public class CreateBookRoomAndAmenityCommand : IRequest<string>
    {
        public int RoomId { get; set; }
        public int AmenityId { get; set; }
        public string BookingCode {  get; set; }
        public string AmenityBookingCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
