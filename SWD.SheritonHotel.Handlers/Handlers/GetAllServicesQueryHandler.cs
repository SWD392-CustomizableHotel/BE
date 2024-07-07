using Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using OtherObjects;
using SWD.SheritonHotel.Data.Repositories.Interfaces;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.Queries;
using SWD.SheritonHotel.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Handlers.Handlers
{
    public class GetAllServicesQueryHandler : IRequestHandler<GetAllServicesQuery, PagedResponse<List<ServiceDto>>>
    {
        private readonly IManageService _manageService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IServiceRepository _serviceRepository;

        public GetAllServicesQueryHandler(IManageService serviceService, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, IServiceRepository serviceRepository)
        {
            _manageService = serviceService;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _serviceRepository = serviceRepository;
        }

        public async Task<PagedResponse<List<ServiceDto>>> Handle(GetAllServicesQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

            //Check User Admin 
            if (user == null || (await _userManager.IsInRoleAsync(user, StaticUserRoles.CUSTOMER)))
            {
                return new PagedResponse<List<ServiceDto>>(new List<ServiceDto>(), 0, 0);
            }

            // Apply filters and search
            var validFilter = request.PaginationFilter;
            var (filterService, totalItems) = await _serviceRepository.GetAllServiceAsync(validFilter.PageNumber, validFilter.PageSize,
                request.ServiceFilter, request.SearchTerm);

            var totalPages = (int)Math.Ceiling(totalItems / (double)request.PaginationFilter.PageSize);
            var response = new PagedResponse<List<ServiceDto>>(filterService, validFilter.PageNumber, validFilter.PageSize)
            {
                TotalRecords = totalItems,
                TotalPages = totalPages,
            };
            return response;
        }
    }
}
