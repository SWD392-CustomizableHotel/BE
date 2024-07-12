using SWD.SheritonHotel.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class Room : BaseEntity
    {
        public string Type { get; set; }
        // Bao gom: Small, Medium, Large
        public string? RoomSize { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string ImagePath { get; set; }
        public string? Image {  get; set; }
        public string? CanvasImage {  get; set; } // Customizing Request
        public int NumberOfPeople { get; set; }

        [ForeignKey("HotelId")]
        public int HotelId { get; set; }
        public virtual Hotel Hotel { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    }
}
