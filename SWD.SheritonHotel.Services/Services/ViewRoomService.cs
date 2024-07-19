using SWD.SheritonHotel.Data.Repositories.Interfaces;
using SWD.SheritonHotel.Domain.Entities;
using SWD.SheritonHotel.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Services.Services
{
    public class ViewRoomService : IViewRoomService
    {
        private readonly IViewRoomRepository _viewRoomRepository;

        public ViewRoomService(IViewRoomRepository viewRoomRepository)
        {
            _viewRoomRepository = viewRoomRepository;
        }
        public async Task<List<Room>> GetAllAvailableRoom()
        {
            return await _viewRoomRepository.GetAllAvailableRoom();
        }
    }
}
