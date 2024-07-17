using Entities;
using MediatR;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.Queries;
using SWD.SheritonHotel.Services.Interfaces;
using SWD.SheritonHotel.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Handlers.Handlers
{
    public class GetAmentityByTypeQueryHandler : IRequestHandler<GetAmenityByTypeQuery, List<Amenity>>
    {
        private readonly IAmenityService _amenityService;
        public GetAmentityByTypeQueryHandler(IAmenityService amenityService)
        {
            _amenityService = amenityService;
        }
        public Task<List<Amenity>> Handle(GetAmenityByTypeQuery request, CancellationToken cancellationToken)
        {
            return _amenityService.GetAmenitiesByTypeAsync(request.AmenityType);
        }
    }
}
