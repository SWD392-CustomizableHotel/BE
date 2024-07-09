using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SWD.SheritonHotel.Data.Context;
using SWD.SheritonHotel.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Data.Repositories
{
    public class ViewRoomRepository : IViewRoomRepository
    {
        private readonly ApplicationDbContext _context;
        public ViewRoomRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Room>> GetAllAvailableRoom()
        {
            return await _context.Room.Where(r => r.Status == "Available" & !r.IsDeleted & r.Type != "Customizable").ToListAsync();
        }
    }
}
