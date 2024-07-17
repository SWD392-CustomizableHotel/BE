using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Domain.DTO
{
    public class FPTResponse
    {
        [JsonPropertyName("errorCode")]
        public int ErrorCode { get; set; }

        [JsonPropertyName("errorMessage")]
        public string ErrorMessage { get; set; }

        [JsonPropertyName("data")]
        public List<FPTIdentityCardData> Data { get; set; }
    }
}
