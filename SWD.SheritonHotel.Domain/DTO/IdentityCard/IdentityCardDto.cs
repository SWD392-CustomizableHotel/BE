using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Domain.DTO.IdentityCard
{
    public class IdentityCardDto
    {
        public string FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string CardNumber { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string Nationality { get; set; }
        public int? PaymentId { get; set; }
    }
}
