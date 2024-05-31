using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class BookingAmenity
    {
        [Key]
        public int BookingAmenityId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [ForeignKey("BookingId")]
        public int BookingId { get; set; }
        public virtual Booking Booking { get; set; }

        [ForeignKey("AmenityId")]
        public int AmenityId { get; set; }
        public virtual Amenity Amenity { get; set; }
    }
}
