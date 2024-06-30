using Microsoft.EntityFrameworkCore;
using SWD.SheritonHotel.Data.Base;
using SWD.SheritonHotel.Data.Context;
using SWD.SheritonHotel.Data.Repositories.Interfaces;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.Entities;
using SWD.SheritonHotel.Domain.OtherObjects;

namespace SWD.SheritonHotel.Data.Repositories
{
    public class ServiceRepository : BaseRepository<AssignedService>, IServiceRepository
    {
        private readonly ApplicationDbContext _context;
        
        public ServiceRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<(List<ServiceDto>, int)> GetAllServicesAsync(int pageNumber, int pageSize, ServiceFilter? serviceFilter, string searchTerm = null)
        {
            var query = _context.Service
                .Include(s => s.Hotel)
                .AsQueryable();

            // Apply filters
            if (serviceFilter != null && !string.IsNullOrEmpty(serviceFilter.ServiceName))
            {
                query = query.Where(s => s.Name.Contains(serviceFilter.ServiceName));
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(s => s.Name.Contains(searchTerm) || s.Description.Contains(searchTerm));
            }

            var totalRecords = await query.CountAsync();

            var services = await query
                .OrderBy(s => s.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(s => new ServiceDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Price = s.Price,
                    Description = s.Description,
                    Status = s.Status,
                    Code = s.Code,
                    HotelId = s.HotelId,
                    UserName = s.AssignedServices
                        .Where(asn => asn.ServiceId == s.Id)
                        .OrderByDescending(asn => asn.AssignedServiceId)
                        .Select(asn => asn.User != null ? asn.User.UserName : null)
                        .FirstOrDefault()
                })
                .ToListAsync();

            return (services, totalRecords);
        }
    }
}
