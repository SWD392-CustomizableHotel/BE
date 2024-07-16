namespace SWD.SheritonHotel.Domain.DTO;

public class BookingDetailsDto
{
    public int BookingId { get; set; }
    public int RoomId { get; set; }
    public string RoomType { get; set; }
    public string RoomDescription { get; set; }
    public int Rating { get; set; }
    public string UserName { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
    public List<ServiceDto> Services { get; set; }
    public List<AmenityDTO> Amenities { get; set; }
    public List<PaymentDto> Payments { get; set; }
    public decimal TotalPrice { get; set; }
}