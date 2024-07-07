using SWD.SheritonHotel.Domain.Entities;
using SWD.SheritonHotel.Domain.OtherObjects;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Entities
{
    public class Service : BaseEntity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public ServiceStatus Status { get; set; }

        [ForeignKey("HotelId")]
        public int HotelId { get; set; }
        public virtual Hotel Hotel { get; set; }

        public virtual ICollection<BookingService> BookingServices { get; set; } = new List<BookingService>();
        [JsonIgnore]
        public virtual ICollection<ApplicationUser> AssignedStaff { get; set; } = new List<ApplicationUser>();
        public virtual ICollection<AssignedService> AssignedServices { get; set; } = new List<AssignedService>();
    }
}
