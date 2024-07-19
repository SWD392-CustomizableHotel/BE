using System.ComponentModel.DataAnnotations;

namespace SWD.SheritonHotel.Domain.DTO.Auth
{
    public class UpdatePermissionDto
    {
        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; }

    }
}
