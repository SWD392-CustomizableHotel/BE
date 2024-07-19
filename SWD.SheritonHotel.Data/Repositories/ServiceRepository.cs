using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SWD.SheritonHotel.Data.Base;
using SWD.SheritonHotel.Data.Context;
using SWD.SheritonHotel.Data.Repositories.Interfaces;
using SWD.SheritonHotel.Domain.DTO.Service;
using SWD.SheritonHotel.Domain.Entities;
using SWD.SheritonHotel.Domain.OtherObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Data.Repositories
{
    public class ServiceRepository : BaseRepository<Service>, IServiceRepository
    {
        private readonly ApplicationDbContext _context;
        protected new readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public ServiceRepository(ApplicationDbContext context, IMapper mapper, UserManager<ApplicationUser> userManager) : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<Service> GetServiceByIdAsync(int serviceId)
        {
            var service = await GetById(serviceId);
            return service ?? throw new KeyNotFoundException($"No service found with ID {serviceId}");
        }

        public async Task<(List<ServiceDto>, int)> GetAllServiceAsync(int pageNumber, int pageSize, ServiceFilter serviceFilter, string searchTerm)
        {
            IQueryable<Service> query = _context.Service
                .Include(s => s.AssignedStaff)
                .AsNoTracking();

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
            var paginatedServices = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(service => new ServiceDto
            {
                Id = service.Id,
                Name = service.Name,
                Description = service.Description,
                Price = service.Price,
                Status = service.Status.ToString(),  // Convert ServiceStatus to string
                HotelId = service.HotelId,
                CreatedBy = service.CreatedBy,
                CreatedDate = service.CreatedDate,
                LastUpdatedBy = service.LastUpdatedBy,
                LastUpdatedDate = service.LastUpdatedDate ?? DateTime.MinValue, // Handle nullable DateTime
                StartDate = service.StartDate ?? DateTime.MinValue, // Handle nullable DateTime
                EndDate = service.EndDate ?? DateTime.MinValue, // Handle nullable DateTime
                AssignedStaff = service.AssignedStaff.Select(staff => new StaffDto
                {
                    Id = staff.Id,
                    FirstName = staff.FirstName,
                    LastName = staff.LastName,
                    Email = staff.Email,
                    UserName = staff.UserName
                }).ToList()
            }).ToListAsync();

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
                service.StartDate = startDate;
                service.EndDate = endDate;
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

        public async Task<bool> AssignStaffToService(int serviceId, List<string> staffIds)
        {
            var service = await _context.Service.Include(s => s.AssignedStaff).FirstOrDefaultAsync(s => s.Id == serviceId);
            if (service == null)
            {
                return false;
            }

            var staff = new List<ApplicationUser>();
            foreach (var staffId in staffIds)
            {
                var user = await _userManager.FindByIdAsync(staffId);
                if (user != null && await _userManager.IsInRoleAsync(user, "STAFF"))
                {
                    staff.Add(user);
                }
            }
            if (staff == null || staff.Count == 0)
            {
                return false;
            }

            service.AssignedStaff.Clear();
            foreach (var member in staff)
            {
                service.AssignedStaff.Add(member);
            }
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task UpdateAsync(Service service)
        {
            _context.Service.Update(service);
            await _context.SaveChangesAsync();
        }

        public void RemoveStaffAssignments(int serviceId)
        {
            var assignments = _context.Set<ServiceStaff>()
                                      .Where(ss => ss.ServiceId == serviceId);

            _context.Set<ServiceStaff>().RemoveRange(assignments);
            _context.SaveChanges();
        }
    }
}
