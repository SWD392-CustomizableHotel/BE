using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }
        public string PaymentCode { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }


        [ForeignKey("BookingId")]
        public int BookingId { get; set; }
        public Booking Booking { get; set; }
    }
}
