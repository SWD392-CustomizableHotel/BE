using SWD.SheritonHotel.Domain.DTO.Responses;
using SWD.SheritonHotel.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Services.Interfaces
{
    public interface IHotelService
    {
        Task<ResponseDto<List<Hotel>>> GetAllHotelsAsync();
        BaseResponse<Hotel> SeedHotelsAsync(List<Hotel> hotels);
    }
}
