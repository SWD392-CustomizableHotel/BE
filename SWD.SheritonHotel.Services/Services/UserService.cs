using Interfaces;
using SWD.SheritonHotel.Data.Repositories.Interfaces;
using SWD.SheritonHotel.Domain.Entities;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        public UserService(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<ApplicationUser> FindUserByEmail(string email)
        {
            return await _userRepo.FindUserByEmail(email);
        }

        public async Task<string> GenResetPasswordTokenAsync(ApplicationUser user)
        {
            return await _userRepo.GeneratePasswordResetTokenAsync(user);
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
                await _userRepo.UpdateAsync(user);
                return true;
            }
            return false;
        }
        
        public async Task UpdateAsync(ApplicationUser user)
        {
            await _userRepo.UpdateAsync(user);
        }

        public async Task<ApplicationUser> GetUserFromJWTAsync(string jWTAsync) 
            => await _userRepo.GetUserFromJWTAsync(jWTAsync);
        public async Task<ApplicationUser> GetUserDetailsByIdAsync(string userId)
            => await _userRepo.GetUserDetailsByIdAsync(userId);

        public async Task<ApplicationUser> GetUserAsync()
        {
            return await _userRepo.GetUserAsync();
        }
    }
}