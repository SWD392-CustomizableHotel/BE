using MediatR;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.OtherObjects;

namespace SWD.SheritonHotel.Domain.Queries;

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