using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SWD.SheritonHotel.Domain.Commands.PaymentCommand.CreatePaymentIntentCommand;

namespace SWD.SheritonHotel.Domain.Commands.PaymentCommand
{
    public class CreatePaymentIntentCustomizableCommand : IRequest<List<string>>
    {
        public Item[] Items { get; set; }

        public class Item
        {
            public string roomId { get; set; }
            public int roomPrice { get; set; }
            public int amenityId { get; set; }
            public int amenityPrice { get; set; }
            public int numberOfDay { get; set; }
            public int numberOfRoom { get; set; }
        }
    }
}
