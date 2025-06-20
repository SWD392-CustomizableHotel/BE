﻿using Microsoft.AspNetCore.Identity;
using SWD.SheritonHotel.Domain.Base;
using SWD.SheritonHotel.Domain.DTO.Account;
using SWD.SheritonHotel.Domain.Entities;
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
        Task<ApplicationUser> GetUserFromJWTAsync(string jWTAsync);
        Task<ApplicationUser> GetUserDetailsByIdAsync(string userId);
        Task<ApplicationUser> GetUserByIdAsync(string staffId);
        Task<List<StaffDTO>> GetUsersByRoleAsync(string role);
        Task<ApplicationUser> GetUserAsync();
	}
}