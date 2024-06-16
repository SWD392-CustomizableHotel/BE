using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SWD.SheritonHotel.Data.Context;
using SWD.SheritonHotel.Data.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public UserRepository(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<ApplicationUser> FindUserByEmail(string email)
        {
            return await _userManager.Users.FirstOrDefaultAsync(s => s.Email.ToLower().Equals(email.ToLower()));
        }

        public async Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            return token;
        }

        public async Task<List<ApplicationUser>> GetAllUsers()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task UpdateAsync(ApplicationUser user)
        {
            try
            {
                await _userManager.UpdateAsync(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
        }
        public async Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
            var users = await _context.Users
                .Where(u => u.Email == email)
                .ToListAsync();

            if (users.Count > 1)
            {
                throw new InvalidOperationException("Sequence contains more than one element.");
            }

            return users.SingleOrDefault();
        }


        public async Task UpdateUserAsync(ApplicationUser user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
