using Entities;
using SWD.SheritonHotel.Data.Repositories.Interfaces;
using SWD.SheritonHotel.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Services.Services
{
    public class BookingRoomService : IBookingService
    {
        private readonly IBookingRoomRepository _bookingRoomRepository;

        public BookingRoomService(IBookingRoomRepository bookingRoomRepository)
        {
            _bookingRoomRepository = bookingRoomRepository;
        }

        public async Task<int> CreateBookingAsync(Booking booking)
        {
            return await _bookingRoomRepository.CreateBookingAsync(booking);
        }
    }
}
