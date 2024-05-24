using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class Service
    {
        [Key]
        public int ServiceId { get; set; }
        public string ServiceCode { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }

        [ForeignKey("HotelId")]
        public int HotelId { get; set; }
        public virtual Hotel Hotel { get; set; }

        public virtual ICollection<BookingService> BookingServices { get; set; } = new List<BookingService>();
    }
}
