using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Domain.DTO
{
    public class RoomDto
    {
        public string RoomNumber { get; set; }
        public string RoomType { get; set; }
        public string RoomStatus { get; set; }

        public decimal RoomPrice { get; set; }

        //public bool IsOccupied { get; set; }
    }
}
