namespace SWD.SheritonHotel.Domain.DTO;

public class BookingHistoryDto
{
    public int BookingId { get; set; }
    public int RoomId { get; set; }
    public int Rating { get; set; }
    public string UserId { get; set; }
    public ICollection<string> Services { get; set; }
    public ICollection<string> Amenities { get; set; }
    public ICollection<decimal> Payments { get; set; }
}