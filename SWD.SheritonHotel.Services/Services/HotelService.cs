using Entities;
using SWD.SheritonHotel.Data.Repositories.Interfaces;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Services.Services
{
    public class HotelService : IHotelService
    {

        private readonly IHotelRepository _hotelRepository;
        public HotelService(IHotelRepository hotelRepository)
        {
            _hotelRepository = hotelRepository;
        }

        public async Task<ResponseDto<List<Hotel>>> GetAllHotelsAsync()
        {
            return await _hotelRepository.GetAllHotelsAsync();
        }
    }
}
