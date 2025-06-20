using System.ComponentModel.DataAnnotations.Schema;

namespace SWD.SheritonHotel.Domain.Entities;

public class AssignedService : BaseEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int AssignedServiceId { get; set; }
    
    [ForeignKey("ApplicationUser")]
    public string UserId { get; set; }
    public virtual ApplicationUser? User { get; set; }

    [ForeignKey("Service")]
    public int ServiceId { get; set; }
    public virtual Service Service { get; set; }
}