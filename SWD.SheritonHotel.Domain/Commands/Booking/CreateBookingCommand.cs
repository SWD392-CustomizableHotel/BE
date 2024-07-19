using MediatR;
using SWD.SheritonHotel.Domain.DTO.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Domain.Commands.Booking
{
    public class CreateBookingCommand : IRequest<ResponseDto<int>>
    {
        public string Code { get; set; }
        public int Rating { get; set; }
        public int RoomId { get; set; }
        public string UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
