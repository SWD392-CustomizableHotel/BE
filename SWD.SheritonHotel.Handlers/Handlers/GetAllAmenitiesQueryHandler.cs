using Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.Queries;
using SWD.SheritonHotel.Services;
using SWD.SheritonHotel.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Handlers.Handlers
{
    public class GetAllAmenitiesQueryHandler : IRequestHandler<GetAllAmenitiesQuery, PagedResponse<List<Amenity>>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAmenityService _amenityService;

        public GetAllAmenitiesQueryHandler(IAmenityService amenityService, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _amenityService = amenityService;
        }
        public async Task<PagedResponse<List<Amenity>>> Handle(GetAllAmenitiesQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            //Check User Admin 
            if (user == null || (await _userManager.IsInRoleAsync(user, "CUSTOMER")))
            {
                return new PagedResponse<List<Amenity>>(new List<Amenity>(), 0, 0);
            }

            // Apply filters and search
            var validFilter = request.PaginationFilter;
            var (filterAmenity, totalItems) = await _amenityService.GetAllAmenityAsync(validFilter.PageNumber, validFilter.PageSize,
                request.AmenityFilter, request.SearchTerm);

            var totalPages = (int)Math.Ceiling(totalItems / (double)request.PaginationFilter.PageSize);
            var response = new PagedResponse<List<Amenity>>(filterAmenity, validFilter.PageNumber, validFilter.PageSize)
            {
                TotalRecords = totalItems,
                TotalPages = totalPages,
            };
            return response;
        }
    }
}
