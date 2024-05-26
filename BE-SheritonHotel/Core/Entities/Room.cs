using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class Room
    {
        [Key]
        public int RoomId { get; set; }
        public string RoomCode { get; set; }
        public string Type { get; set; }
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }

        [ForeignKey("HotelId")]
        public int HotelId { get; set; }
        public virtual Hotel Hotel { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
