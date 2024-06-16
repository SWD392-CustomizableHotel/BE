using SWD.SheritonHotel.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class Booking : BaseEntity
    {
        public int Rating { get; set; }

        [ForeignKey("RoomId")]
        public int RoomId { get; set; }
        public virtual Room Room { get; set; }

        [ForeignKey("UserId")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public virtual ICollection<BookingService> BookingServices { get; set; } =
            new List<BookingService>();
        public virtual ICollection<BookingAmenity> BookingAmenities { get; set; } =
            new List<BookingAmenity>();
    }
}
