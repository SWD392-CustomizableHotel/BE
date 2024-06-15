using Entities;
using Interfaces;
using SWD.SheritonHotel.Data.Repositories.Interfaces;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        public UserService(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<List<ApplicationUser>> GetAllUsers()
        {
            return await _userRepo.GetAllUsers();
        }
        
        public async Task<bool> VerifyEmailTokenAsync(string email, string token)
        {
            var user = await _userRepo.GetUserByEmailAsync(email);

            if (user != null && user.VerifyToken == token && !user.isActived)
            {
                user.isActived = true;
                user.VerifyToken = null; 
                user.VerifyTokenExpires = DateTime.MinValue;
                await _userRepo.UpdateUserAsync(user);
                return true;
            }

            return false;
        }
        
    }
}
