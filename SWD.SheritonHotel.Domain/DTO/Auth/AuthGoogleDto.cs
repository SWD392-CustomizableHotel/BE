using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// using Newtonsoft.Json;

namespace SWD.SheritonHotel.Domain.DTO.Auth
{
    public class AuthGoogleDto
    {
        public string? Provider { get; set; }
        public string? IdToken { get; set; }
    }
}
