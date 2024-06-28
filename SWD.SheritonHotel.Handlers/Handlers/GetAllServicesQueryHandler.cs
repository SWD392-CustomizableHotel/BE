using MediatR;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.Queries;
using SWD.SheritonHotel.Services.Interfaces;

namespace SWD.SheritonHotel.Handlers.Handlers
{
    public class GetAllServicesQueryHandler : IRequestHandler<GetAllServicesQuery, PagedResponse<List<ServiceDto>>>
    {
        private readonly IServiceService _serviceService;

        public GetAllServicesQueryHandler(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        public async Task<PagedResponse<List<ServiceDto>>> Handle(GetAllServicesQuery request, CancellationToken cancellationToken)
        {
            var validFilter = request.PaginationFilter;
            var (services, totalRecords) = await _serviceService.GetAllServicesAsync(validFilter.PageNumber, validFilter.PageSize, request.ServiceFilter, request.SearchTerm);
            var totalPages = (int)Math.Ceiling(totalRecords / (double)validFilter.PageSize);
            // Create list of ServiceDto
            var serviceDtos = services.Select(s => new ServiceDto
            {
                Id = s.Id,
                Name = s.Name,
                Price = s.Price,
                Description = s.Description,
                Status = s.Status,
                Code = s.Code,
                HotelId = s.HotelId,
                UserId = s.UserId,
                UserName = s.UserName
            }).ToList();
            var response = new PagedResponse<List<ServiceDto>>(serviceDtos, validFilter.PageNumber, validFilter.PageSize)
            {
                TotalRecords = totalRecords,
                TotalPages = totalPages,
            };

            return response;
        }
    }
}