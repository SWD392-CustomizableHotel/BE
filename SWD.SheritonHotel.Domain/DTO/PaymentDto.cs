namespace SWD.SheritonHotel.Domain.DTO;

public class PaymentDto
{
    public int Id { get; set; }
    public Decimal Amount { get; set; }
    public string Status { get; set; }
    public string PaymentIntentId { get; set; }
    public int BookingId { get; set; }
    public string Code { get; set; }
    public string CreatedBy { get; set; }
    public DateTime PaymentDate { get; set; }
}