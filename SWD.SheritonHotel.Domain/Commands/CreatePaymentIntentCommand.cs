using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Domain.Commands
{
    public class CreatePaymentIntentCommand : IRequest<List<string>>
    {
        public Item[] Items { get; set; }
        public class Item
        {
            public string RoomId { get; set; }
            public int RoomPrice { get; set; }
            public int NumberOfDate { get; set; }
            public int NumberOfRoom { get; set; }
            public string UserEmail { get; set; }
            public string UserName { get; set; }
        }
    }
}
