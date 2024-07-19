using MediatR;
using SWD.SheritonHotel.Domain.DTO.Responses;
using SWD.SheritonHotel.Domain.DTO.Service;
using SWD.SheritonHotel.Domain.OtherObjects;

namespace SWD.SheritonHotel.Domain.Queries.ServiceQuery;

public class GetAllAssignServicesQuery : IRequest<PagedResponse<List<ServiceListDto>>>
{
    public PaginationFilter PaginationFilter { get; }
    public string SearchTerm { get; }
    public AssignServiceFilter AssignServiceFilter { get; }
    public GetAllAssignServicesQuery(PaginationFilter paginationFilter, AssignServiceFilter serviceFilter, string searchTerm)
    {
        PaginationFilter = paginationFilter ?? new PaginationFilter();
        SearchTerm = searchTerm;
        AssignServiceFilter = serviceFilter ?? new AssignServiceFilter();
    }
}