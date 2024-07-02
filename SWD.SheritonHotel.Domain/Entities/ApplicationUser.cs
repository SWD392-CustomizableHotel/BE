using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
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
        [NotMapped]
        public IFormFile? Certificate { get; set; }
        public string? CertificatePath { get; set; }
        public string? Address { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public virtual ICollection<AssignedService> AssignedServices { get; set; } = new List<AssignedService>();
    }
}
