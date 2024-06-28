namespace SWD.SheritonHotel.Domain.DTO;

public class AssignServiceDto
{
    public string UserId { get; set; }
    public int ServiceId { get; set; }
    public string Code { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string LastUpdatedBy { get; set; }
    public DateTime LastUpdatedDate { get; set; }
    
}