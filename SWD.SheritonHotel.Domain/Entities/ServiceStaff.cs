using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Domain.Entities
{
    public class ServiceStaff
    {
        public int ServiceId { get; set; }
        public string UserId { get; set; }
        public Service Service { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }

}
