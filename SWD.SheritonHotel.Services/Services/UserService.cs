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
    }
}
