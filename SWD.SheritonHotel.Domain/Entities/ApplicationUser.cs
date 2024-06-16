    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Microsoft.AspNetCore.Identity;
    using SWD.SheritonHotel.Domain.Entities;

namespace Entities
{
    public class ApplicationUser : IdentityUser<string>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? Dob { get; set; }
        public string? VerifyToken { get; set; }
        public DateTime? VerifyTokenExpires { get; set; }
        public bool isActived { get; set; } = false;
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
