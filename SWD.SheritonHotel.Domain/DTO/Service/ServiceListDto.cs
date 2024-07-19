using SWD.SheritonHotel.Domain.OtherObjects;

namespace SWD.SheritonHotel.Domain.DTO.Service;

public class ServiceListDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public ServiceStatus Status { get; set; }
    public string Code { get; set; }
    public int HotelId { get; set; }
    public string UserId { get; set; }
    public string UserName { get; set; }
}