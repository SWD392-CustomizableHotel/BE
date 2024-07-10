using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
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
        public string? CertificatePath { get; set; }
        public string? Address { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public virtual IdentityCard IdentityCard { get; set; }
        [JsonIgnore]
        public virtual ICollection<Service> AssignedServiceS { get; set; } = new List<Service>();
        public virtual ICollection<AssignedService> AssignedServices { get; set; } = new List<AssignedService>();
    }
}
