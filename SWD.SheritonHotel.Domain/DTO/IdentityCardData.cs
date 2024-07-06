using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Domain.DTO
{
    public class FPTIdentityCardData
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("id_prob")]
        public string IdProb { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("name_prob")]
        public string NameProb { get; set; }

        [JsonPropertyName("dob")]
        public string Dob { get; set; }

        [JsonPropertyName("dob_prob")]
        public string DobProb { get; set; }

        [JsonPropertyName("sex")]
        public string Sex { get; set; }

        [JsonPropertyName("sex_prob")]
        public string SexProb { get; set; }

        [JsonPropertyName("nationality")]
        public string Nationality { get; set; }

        [JsonPropertyName("nationality_prob")]
        public string NationalityProb { get; set; }

        [JsonPropertyName("home")]
        public string Home { get; set; }

        [JsonPropertyName("home_prob")]
        public string HomeProb { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; }

        [JsonPropertyName("address_prob")]
        public string AddressProb { get; set; }

        [JsonPropertyName("doe")]
        public string Doe { get; set; }

        [JsonPropertyName("doe_prob")]
        public string DoeProb { get; set; }

        [JsonPropertyName("overall_score")]
        public string OverallScore { get; set; }

        [JsonPropertyName("number_of_name_lines")]
        public string NumberOfNameLines { get; set; }

        [JsonPropertyName("address_entities")]
        public AddressEntities AddressEntities { get; set; }

        [JsonPropertyName("type_new")]
        public string TypeNew { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }
}
