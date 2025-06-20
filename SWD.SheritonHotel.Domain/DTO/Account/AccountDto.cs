namespace SWD.SheritonHotel.Domain.DTO.Account;

public class AccountDto
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime? Dob { get; set; }
    public bool IsActive { get; set; } = true;
    public List<string> Roles { get; set; }
}