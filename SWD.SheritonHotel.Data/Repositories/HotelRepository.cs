using static Microsoft.EntityFrameworkCore.DbContext;
using Entities;
using Microsoft.EntityFrameworkCore;
using SWD.SheritonHotel.Data.Repositories.Interfaces;
using SWD.SheritonHotel.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWD.SheritonHotel.Data.Context;
using SWD.SheritonHotel.Data.Base;

namespace SWD.SheritonHotel.Data.Repositories
{
    public class HotelRepository : BaseRepository<Hotel>, IHotelRepository
    {
        private readonly ApplicationDbContext _context;

        public HotelRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Hotel> GetByIdAsync(int id)
        {
            return await base.GetById(id);
        }

        public async Task<ResponseDto<List<Hotel>>> GetAllHotelsAsync()
        {
            var hotels = await _context.Hotel.ToListAsync();
            return new ResponseDto<List<Hotel>>
            {
                Data = hotels,
                IsSucceeded = true,
                Message = "Hotels retrieved successfully"
            };
        }

    }
}
