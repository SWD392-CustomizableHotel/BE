using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Domain.DTO
{
    public class NotificationDto
    {
        public string UserName { get; set; }
        public string RoomType { get; set; }
        public decimal Amount { get; set; }
        public DateTime BookingDate { get; set; }
    }
}
