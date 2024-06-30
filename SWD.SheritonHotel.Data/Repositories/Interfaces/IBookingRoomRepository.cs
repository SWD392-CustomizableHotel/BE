using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Data.Repositories.Interfaces
{
    public interface IBookingRoomRepository
    {
        Task<int> CreateBookingAsync(Booking booking);  
    }
}
