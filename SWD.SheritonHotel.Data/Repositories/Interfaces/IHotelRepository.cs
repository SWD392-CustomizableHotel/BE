using Entities;
using SWD.SheritonHotel.Domain.Base;
using SWD.SheritonHotel.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Data.Repositories.Interfaces
{
    public interface IHotelRepository : IBaseRepository<Hotel>
    {
        Task<Hotel> GetByIdAsync(int id);
        Task<ResponseDto<List<Hotel>>> GetAllHotelsAsync();
    }
}
