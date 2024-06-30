using MediatR;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.OtherObjects;

namespace SWD.SheritonHotel.Domain.Queries;

public class GetAllServicesQuery : IRequest<PagedResponse<List<ServiceDto>>>
{
    public PaginationFilter PaginationFilter { get; }
    public string SearchTerm { get; }
    public ServiceFilter ServiceFilter { get; }
    public GetAllServicesQuery(PaginationFilter paginationFilter, ServiceFilter serviceFilter, string searchTerm)
    {
        PaginationFilter = paginationFilter ?? new PaginationFilter();
        SearchTerm = searchTerm;
        ServiceFilter = serviceFilter ?? new ServiceFilter();
    }
}