using SWD.SheritonHotel.Domain.OtherObjects;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWD.SheritonHotel.Domain.Entities
{
    public class Amenity : BaseEntity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        // Bao gom: Basic, Advanced, Family
        public string? AmenityType { get; set; }
        public string Description { get; set; }
        public int Capacity { get; set; }
        public int InUse { get; set; }
        public AmenityStatus Status { get; set; }

        [ForeignKey("HotelId")]
        public int HotelId { get; set; }
        public Hotel Hotel { get; set; }

        public virtual ICollection<BookingAmenity> BookingAmenities { get; set; } =
            new List<BookingAmenity>();
    }
}
