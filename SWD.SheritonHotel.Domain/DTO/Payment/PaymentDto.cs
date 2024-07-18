namespace SWD.SheritonHotel.Domain.DTO.Payment;

public class PaymentDto
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public string Status { get; set; }
    public string PaymentIntentId { get; set; }
    public int BookingId { get; set; }
    public string Code { get; set; }
    public string CreatedBy { get; set; }
    public DateTime PaymentDate { get; set; }
    public string PaymentMethod { get; set; }
}