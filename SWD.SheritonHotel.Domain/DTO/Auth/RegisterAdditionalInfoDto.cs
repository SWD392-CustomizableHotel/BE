using System.ComponentModel.DataAnnotations;

namespace SWD.SheritonHotel.Domain.DTO.Auth;

public class RegisterAdditionalInfoDto
{
    [Required]
    public string UserName { get; set; }

    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [Required]
    [Phone]
    public string PhoneNumber { get; set; }
}