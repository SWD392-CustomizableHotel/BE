using System.ComponentModel.DataAnnotations;

namespace SWD.SheritonHotel.Domain.DTO.Account;

public class VerifyTokenDto
{
    public class VerifyAccountDto
    {
        [Required(ErrorMessage = "FirstName is required")]
        public string Token { get; set; }
    }

}