using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class Role : IdentityRole<int>
    {
        [Key]
        public int RoleId { get; set; }
        public string? NormalizeName { get; set; }
        public string? RoleName { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
    }
}
