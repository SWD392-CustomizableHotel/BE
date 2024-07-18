using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWD.SheritonHotel.Domain.Entities
{
    public class BookingService : BaseEntity
    {

        [ForeignKey("BookingId")]
        [Column(Order = 0)]
        public int BookingId { get; set; }
        public virtual Booking Booking { get; set; }

        [ForeignKey("ServiceId")]
        [Column(Order = 1)]
        public int ServiceId { get; set; }
        public virtual Service Service { get; set; }
    }
}
