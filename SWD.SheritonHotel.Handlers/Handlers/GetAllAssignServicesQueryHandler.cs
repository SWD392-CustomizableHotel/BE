using MediatR;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.OtherObjects;
using SWD.SheritonHotel.Domain.Queries;
using SWD.SheritonHotel.Services.Interfaces;

namespace SWD.SheritonHotel.Handlers.Handlers;

public class GetAllAssignServicesQueryHandler : IRequestHandler<GetAllAssignServicesQuery, PagedResponse<List<ServiceListDto>>>
{
    private readonly IAssignServiceService _serviceService;

    public GetAllAssignServicesQueryHandler(IAssignServiceService serviceService)
    {
        _serviceService = serviceService;
    }

    public async Task<PagedResponse<List<ServiceListDto>>> Handle(GetAllAssignServicesQuery request, CancellationToken cancellationToken)
    {
        var validFilter = request.PaginationFilter;
        var (services, totalRecords) = await _serviceService.GetAllServicesAsync(validFilter.PageNumber, validFilter.PageSize, request.AssignServiceFilter, request.SearchTerm);
        var totalPages = (int)Math.Ceiling(totalRecords / (double)validFilter.PageSize);
        // Create list of ServiceDto
        var serviceDtos = services.Select(s => new ServiceListDto()
        {
            Id = s.Id,
            Name = s.Name,
            Price = s.Price,
            Description = s.Description,
            Status = s.Status,
            Code = s.Code,
            HotelId = s.HotelId,
            UserName = s.UserName
        }).ToList();
        var response = new PagedResponse<List<ServiceListDto>>(serviceDtos, validFilter.PageNumber, validFilter.PageSize)
        {
            TotalRecords = totalRecords,
            TotalPages = totalPages,
        };

        return response;
    }
}