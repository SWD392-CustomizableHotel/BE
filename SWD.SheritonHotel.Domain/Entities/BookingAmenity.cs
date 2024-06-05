using SWD.SheritonHotel.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class BookingAmenity : BaseEntity
    {
        [ForeignKey("BookingId")]
        public int BookingId { get; set; }
        public virtual Booking Booking { get; set; }

        [ForeignKey("AmenityId")]
        public int AmenityId { get; set; }
        public virtual Amenity Amenity { get; set; }
    }
}
