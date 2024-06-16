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

        [ForeignKey("BookingId")]
        public int BookingId { get; set; }
        public Booking Booking { get; set; }
    }
}
