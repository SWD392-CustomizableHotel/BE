using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Domain.DTO
{
    public class AddressEntities
    {
        [JsonPropertyName("province")]
        public string Province { get; set; }

        [JsonPropertyName("district")]
        public string District { get; set; }

        [JsonPropertyName("ward")]
        public string Ward { get; set; }

        [JsonPropertyName("street")]
        public string Street { get; set; }
    }
}
