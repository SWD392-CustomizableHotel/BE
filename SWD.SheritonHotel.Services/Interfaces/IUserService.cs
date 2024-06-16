using Entities;

namespace Interfaces
{
    public interface IUserService
    {
        Task<List<ApplicationUser>> GetAllUsers();
        Task<ApplicationUser> FindUserByEmail(string email);
        Task<string> GenResetPasswordTokenAsync(ApplicationUser user);
        Task UpdateAsync(ApplicationUser user);
        
        Task<bool> VerifyEmailTokenAsync(string email, string token);
    }
}
