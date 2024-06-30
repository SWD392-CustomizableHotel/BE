using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Services.Interfaces
{
    public interface IBookingService
    {
        Task<int> CreateBookingAsync(Booking booking);
    }
}
