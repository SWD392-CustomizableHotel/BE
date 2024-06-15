using Entities;

namespace Interfaces
{
    public interface IUserService
    {
        Task<List<ApplicationUser>> GetAllUsers();
        
        Task<bool> VerifyEmailTokenAsync(string email, string token);
    }
}
