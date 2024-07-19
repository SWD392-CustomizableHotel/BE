using MediatR;
using SWD.SheritonHotel.Domain.DTO.Responses;
using SWD.SheritonHotel.Domain.DTO.Service;
using SWD.SheritonHotel.Domain.OtherObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Domain.Queries.ServiceQuery
{
    public class GetAllServicesQuery : IRequest<PagedResponse<List<ServiceDto>>>
    {
        public PaginationFilter PaginationFilter { get; set; }

        public ServiceFilter ServiceFilter { get; set; }

        public string SearchTerm { get; set; }

        public GetAllServicesQuery(PaginationFilter paginationFilter, ServiceFilter serviceFilter, string searchTerm)
        {
            PaginationFilter = paginationFilter ?? new PaginationFilter();
            ServiceFilter = serviceFilter ?? new ServiceFilter();
            SearchTerm = searchTerm;
        }
    }
}
