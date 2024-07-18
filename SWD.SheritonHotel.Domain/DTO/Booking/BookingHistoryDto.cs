using SWD.SheritonHotel.Domain.DTO.Amenity;
using SWD.SheritonHotel.Domain.DTO.Payment;
using SWD.SheritonHotel.Domain.DTO.Service;

namespace SWD.SheritonHotel.Domain.DTO.Booking;

public class BookingHistoryDto
{
    public int BookingId { get; set; }
    public string RoomType { get; set; }
    public string RoomDescription { get; set; }
    public int Rating { get; set; }
    public string UserName { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public List<ServiceDto> Services { get; set; }
    public List<AmenityDTO> Amenities { get; set; }
    public List<PaymentDto> Payments { get; set; }
}