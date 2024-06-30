using Entities;
using Microsoft.AspNetCore.Identity;
using SWD.SheritonHotel.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Data.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<List<ApplicationUser>> GetAllUsers();
        Task<ApplicationUser> FindUserByEmail(string email);
        Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user);
        Task UpdateAsync(ApplicationUser user);
        Task<ApplicationUser> GetUserByEmailAsync(string email);
        Task<IdentityResult> UpdateUserAsync(ApplicationUser user);
    }
}