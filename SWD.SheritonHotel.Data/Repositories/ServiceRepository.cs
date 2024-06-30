using AutoMapper;
using Entities;
using Microsoft.EntityFrameworkCore;
using SWD.SheritonHotel.Data.Base;
using SWD.SheritonHotel.Data.Context;
using SWD.SheritonHotel.Data.Repositories.Interfaces;
using SWD.SheritonHotel.Domain.OtherObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Data.Repositories
{
    public class ServiceRepository : BaseRepository<Service>, IServiceRepository
    {
        private readonly ApplicationDbContext _context;
        protected new readonly IMapper _mapper;

        public ServiceRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<Service> GetServiceByIdAsync(int serviceId)
        {
            var service = await GetById(serviceId);
            return service ?? throw new KeyNotFoundException($"No service found with ID {serviceId}");
        }

        public async Task<(List<Service>, int)> GetAllServiceAsync(int pageNumber, int pageSize,
                    ServiceFilter? serviceFilter, string searchTerm = null)
        {
            IQueryable<Service> query = DbSet.AsQueryable().AsNoTracking();

            query = query.Where(a => !a.IsDeleted);

            // Filter
            if (serviceFilter != null)
            {
                if (serviceFilter.ServiceStatus.HasValue)
                {
                    query = query.Where(r => r.Status == serviceFilter.ServiceStatus.Value);
                }

                if (serviceFilter.HotelId != 0)
                {
                    query = query.Where(r => r.HotelId == serviceFilter.HotelId);
                }
            }

            // Search
            if (!string.IsNullOrEmpty(searchTerm))
            {
                ServiceStatus? statusSearch = null;
                if (Enum.TryParse<ServiceStatus>(searchTerm, true, out var parsedStatus))
                {
                    statusSearch = parsedStatus;
                }

                query = query.Where(r => r.Code.Contains(searchTerm) ||
                                          r.Description.Contains(searchTerm) ||
                                          (statusSearch.HasValue && r.Status == statusSearch.Value));
            }

            var totalItems = await query.CountAsync();
            var paginatedServices = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return (paginatedServices, totalItems);
        }

        public async Task<Service> CreateServiceAsync(Service service)
        {
            Add(service);
            await _context.SaveChangesAsync();
            return service;
        }

        public async Task<Service> UpdateServiceAsync(int serviceId,
            string name,
            string description,
            decimal price,
            DateTime startDate, 
            DateTime endDate,
            string updatedBy)
        {
            var service = await GetById(serviceId);
            if (service != null)
            {
                service.Name = name;
                service.Description = description;
                service.Price = price;
                service.StartDate = startDate.Date;
                service.EndDate = endDate.Date;
                service.LastUpdatedDate = DateTime.Now;
                service.LastUpdatedBy = updatedBy;
                Update(service);
                await _context.SaveChangesAsync();
                return service;
            }
            else
            {
                throw new KeyNotFoundException($"No service found with ID {serviceId}");
            }
        }

        public async Task<Service> UpdateServiceStatus(int serviceId, string status, string updatedBy)
        {
            var service = await GetById(serviceId);
            if (service != null)
            {
                if (Enum.TryParse(status, true, out ServiceStatus parsedStatus))
                {
                    service.Status = parsedStatus;
                    service.LastUpdatedBy = updatedBy;
                    Update(service);
                    await _context.SaveChangesAsync();
                    return service;
                }
                else
                {
                    throw new ArgumentException($"Invalid status value: {status}");
                }
            }
            else
            {
                throw new KeyNotFoundException($"No service found with ID {serviceId}");
            }
        }

        public async Task DeleteServiceAsync(int serviceId)
        {
            var service = await GetById(serviceId);
            if (service != null)
            {
                Delete(service);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException($"No service found with ID {serviceId}");
            }
        }

        public async Task<List<Service>> GetServicesByRoomIdAsync(int roomId)
        {
            var room = await GetQueryable<Room>().Include(r => r.Hotel).FirstOrDefaultAsync(r => r.Id == roomId);
            if (room == null)
            {
                throw new KeyNotFoundException($"No room found with ID {roomId}");
            }

            var services = await GetQueryable<Service>()
                            .Where(a => a.HotelId == room.HotelId && !a.IsDeleted)
                            .ToListAsync();
            return services;
        }
    }
}
