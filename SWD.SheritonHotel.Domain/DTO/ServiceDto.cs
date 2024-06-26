namespace SWD.SheritonHotel.Domain.DTO;

public class ServiceDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    public int HotelId { get; set; }
}