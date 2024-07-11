using Entities;
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
    public class BookingAmenityRepository : BaseRepository<BookingAmenity>, IBookingAmenityRepository
    {
        private readonly ApplicationDbContext _context;
        public BookingAmenityRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
