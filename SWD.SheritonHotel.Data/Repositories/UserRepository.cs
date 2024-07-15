using AutoMapper;
using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SWD.SheritonHotel.Data.Context;
using SWD.SheritonHotel.Data.Repositories.Interfaces;
using SWD.SheritonHotel.Domain.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UserRepository(UserManager<ApplicationUser> userManager, ApplicationDbContext context, IMapper mapper)
        {
            _userManager = userManager;
            _context = context;
            _mapper = mapper;
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
        public async Task<IdentityResult> UpdateUserAsync(ApplicationUser user)
        {
            return await _userManager.UpdateAsync(user);
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string staffId)
        {
            return await _context.Users.FindAsync(staffId);
        }
        public async Task<List<StaffDTO>> GetUsersByRoleAsync(string role)
        {
            var users = await _userManager.Users.ToListAsync();
            var staff = new List<ApplicationUser>();

            foreach (var user in users)
            {
                if (await _userManager.IsInRoleAsync(user, role))
                {
                    staff.Add(user);
                }
            }

            return _mapper.Map<List<StaffDTO>>(staff);
        }

		public async Task<List<CustomerDTO>> GetCustomerByRoleAsync(string role)
		{
			var users = await _userManager.Users.ToListAsync();
			var customer = new List<ApplicationUser>();

			foreach (var user in users)
			{
				if (await _userManager.IsInRoleAsync(user, role))
				{
					customer.Add(user);
				}
			}

			return _mapper.Map<List<CustomerDTO>>(customer);
		}

	}
}