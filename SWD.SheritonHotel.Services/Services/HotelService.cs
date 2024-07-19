using SWD.SheritonHotel.Data.Repositories.Interfaces;
using SWD.SheritonHotel.Domain.DTO.Responses;
using SWD.SheritonHotel.Domain.Entities;
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

        public BaseResponse<Hotel> SeedHotelsAsync(List<Hotel> hotels)
        {
            _hotelRepository.AddRange(hotels);
            _hotelRepository.SaveChanges();

            return new BaseResponse<Hotel>
            {
                Results = hotels,
                IsSucceed = true,
                Message = "Hotels seeded successfully"
            };
        }

        public BaseResponse<Hotel> SeedDefaultHotels()
        {
            var hotels = new List<Hotel>
            {
                new Hotel
                {
                    Code = "Hotel 1",
                    Address = "123 Main St",
                    Phone = "123-456-7890",
                    Description = "A lovely hotel in the heart of the city",
                },
                new Hotel
                {
                    Code = "Hotel 2",
                    Address = "456 Elm St",
                    Phone = "987-654-3210",
                    Description = "A cozy hotel near the park",
                }
            };

            return SeedHotelsAsync(hotels);
        }
    }
}
