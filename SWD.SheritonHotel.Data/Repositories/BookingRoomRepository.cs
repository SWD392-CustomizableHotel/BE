using AutoMapper;
using Entities;
using Microsoft.EntityFrameworkCore;
using SWD.SheritonHotel.Data.Base;
using SWD.SheritonHotel.Data.Context;
using SWD.SheritonHotel.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Data.Repositories
{
    public class BookingRoomRepository : BaseRepository<Booking>, IBookingRoomRepository
    {
        private readonly ApplicationDbContext _context;
        protected readonly IMapper _mapper;

        public BookingRoomRepository(ApplicationDbContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> CreateBookingAsync(Booking booking)
        {
            Add(booking);
            await _context.SaveChangesAsync();
            return booking.Id;
        }
    }
}
