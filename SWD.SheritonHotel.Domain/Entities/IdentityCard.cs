using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Domain.Entities
{
    public class IdentityCard : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string FullName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [MaxLength(12)]
        public string CardNumber { get; set; }

        [Required]
        [MaxLength(9)]
        public string Gender { get; set; }

        [Required]
        [MaxLength(100)]
        public string Address { get; set; }

        [Required]
        [MaxLength(50)]
        public string Nationality { get; set; }

        [ForeignKey("PaymentId")]
        public int? PaymentId { get; set; }
        public virtual Payment Payment { get; set; }
    }
}
