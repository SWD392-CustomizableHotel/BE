using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Domain.DTO
{
    public class CreatePaymentIntentDTO
    {
        public Item[] Items { get; set; }

        public class Item
        {
            public string RoomId { get; set; }
            public int RoomPrice { get; set; }
            public int NumberOfDate { get; set; }
            public int NumberOfRoom { get; set; }
            public int UserEmail { get; set; }
            public it UserName { get; set; }
        }
    }
}
