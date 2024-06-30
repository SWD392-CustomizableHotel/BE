using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Domain.Commands
{
    public class CreatePaymentIntentCommand : IRequest<string>
    {
        public Item[] Items { get; set; }

        public class Item
        {
            public string Id { get; set; }
            public int Amount { get; set; }
        }
    }
}
