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
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Handlers.Handlers
{
    public class GetAllServicesQueryHandler : IRequestHandler<GetAllServicesQuery, PagedResponse<List<Service>>>
    {
        private readonly IManageService _manageService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetAllServicesQueryHandler(IManageService serviceService, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _manageService = serviceService;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<PagedResponse<List<Service>>> Handle(GetAllServicesQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

            //Check User Admin 
            if (user == null || (await _userManager.IsInRoleAsync(user, StaticUserRoles.CUSTOMER)))
            {
                return new PagedResponse<List<Service>>(new List<Service>(), 0, 0);
            }

            // Apply filters and search
            var validFilter = request.PaginationFilter;
            var (filterService, totalItems) = await _manageService.GetAllServiceAsync(validFilter.PageNumber, validFilter.PageSize,
                request.ServiceFilter, request.SearchTerm);

            var totalPages = (int)Math.Ceiling(totalItems / (double)request.PaginationFilter.PageSize);
            var response = new PagedResponse<List<Service>>(filterService, validFilter.PageNumber, validFilter.PageSize)
            {
                TotalRecords = totalItems,
                TotalPages = totalPages,
            };
            return response;
        }
    }
}
