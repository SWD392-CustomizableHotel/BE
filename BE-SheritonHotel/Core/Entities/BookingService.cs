using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class BookingService
    {
        [Key]
        public int BookingServiceId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

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
