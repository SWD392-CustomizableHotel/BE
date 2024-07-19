using MediatR;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Domain.Queries.AmenityQuery
{
    public class GetAmenityByTypeQuery : IRequest<List<Amenity>>
    {
        public string AmenityType { get; set; }
    }
}
