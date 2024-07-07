namespace SWD.SheritonHotel.Domain.DTO;

public class BookingHistoryDto
{
    public int BookingId { get; set; }
    
    public string RoomType { get; set; }
    public string RoomDescription { get; set; }
    public int Rating { get; set; }
    public string UserName { get; set; }
    public ICollection<string> Services { get; set; }
    public ICollection<string> Amenities { get; set; }
    public ICollection<decimal> Payments { get; set; }
}