using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class Hotel
    {
        [Key]
        public int HotelId { get; set; }
        public string HotelCode { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
        public virtual ICollection<Service> Services { get; set; } = new List<Service>();
        public virtual ICollection<Amenity> Amenities { get; set; } = new List<Amenity>();
    }
}
