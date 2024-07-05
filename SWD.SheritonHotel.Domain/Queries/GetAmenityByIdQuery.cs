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
    public class GetAmenityByIdQuery : IRequest<ResponseDto<Amenity>>
    {
        public int AmenityId { get; set; }

        public GetAmenityByIdQuery(int amenityId)
        {
            AmenityId = amenityId;
        }
    }
}
