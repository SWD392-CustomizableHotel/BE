using Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Domain.Commands
{
    public class SeedsHotelCommand : IRequest<List<Hotel>>
    {
        public List<Hotel> Hotel { get; set; }
        public SeedsHotelCommand(List<Hotel> hotel) {
            Hotel = hotel; 
        }
    }
}
