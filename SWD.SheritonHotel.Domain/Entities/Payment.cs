using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using SWD.SheritonHotel.Domain.Entities;

namespace Entities
{
    public class Payment : BaseEntity
    {
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public string PaymentIntentId { get; set; }
        
        public string PaymentMethod { get; set; }

        [ForeignKey("BookingId")]
        public int BookingId { get; set; }
        public virtual Booking Booking { get; set; }
        public virtual IdentityCard IdentityCard { get; set; }
    }
}
