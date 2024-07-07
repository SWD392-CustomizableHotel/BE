using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Domain.OtherObjects
{
    public class ServiceFilter
    {
        public ServiceStatus? ServiceStatus { get; set; }
        public int HotelId { get; set; }
    }
}
