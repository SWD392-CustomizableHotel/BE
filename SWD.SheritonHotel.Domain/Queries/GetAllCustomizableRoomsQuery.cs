using Entities;
using MediatR;
using SWD.SheritonHotel.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Domain.Queries
{
    public class GetAllCustomizableRoomsQuery : IRequest<List<Room>>
    {
        public string? roomSize {  get; set; }
        public int? numberOfPeople {  get; set; }
    }
}
