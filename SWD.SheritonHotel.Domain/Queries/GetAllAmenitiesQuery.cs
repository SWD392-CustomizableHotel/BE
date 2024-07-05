using Entities;
using MediatR;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.OtherObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Domain.Queries
{
    public class GetAllAmenitiesQuery : IRequest<PagedResponse<List<Amenity>>>
    {
        public PaginationFilter PaginationFilter { get; set; }

        public AmenityFilter AmenityFilter { get; set; }

        public string SearchTerm { get; set; }
        public GetAllAmenitiesQuery(PaginationFilter paginationFilter, AmenityFilter amenityFilter, string searchTerm)
        {
            PaginationFilter = paginationFilter ?? new PaginationFilter();
            AmenityFilter = amenityFilter ?? new AmenityFilter();
            SearchTerm = searchTerm;
        }
    }
}
