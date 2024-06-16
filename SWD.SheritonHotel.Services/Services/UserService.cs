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

        public async Task UpdateAsync(ApplicationUser user)
        {
            await _userRepo.UpdateAsync(user);
        }
    }
}