using AutoMapper;
using Entities;
using SWD.SheritonHotel.Data.Repositories.Interfaces;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.OtherObjects;
using SWD.SheritonHotel.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Services.Services
{
    public class ManageServiceService : IManageService
    {
        private readonly IServiceRepository _serviceRepository;
        protected new readonly IMapper _mapper;

        public ManageServiceService(IServiceRepository serviceRepository, IMapper mapper)
        {
            _serviceRepository = serviceRepository;
            _mapper = mapper;
        }

        public async Task<Service> GetServiceByIdAsync(int serviceId)
        {
            return await _serviceRepository.GetServiceByIdAsync(serviceId);
        }

        public async Task<(List<ServiceDto>, int)> GetAllServiceAsync(int pageNumber, int pageSize, ServiceFilter? serviceFilter, string searchTerm = null)
        {
            var (services, totalItems) = await _serviceRepository.GetAllServiceAsync(pageNumber, pageSize, serviceFilter, searchTerm);

            var serviceDtos = _mapper.Map<List<ServiceDto>>(services);

            return (serviceDtos, totalItems);
        }

        public async Task<Service> CreateServiceAsync(Service service)
        {
            var newService = await _serviceRepository.CreateServiceAsync(service);
            return newService;
        }

        public async Task<Service> UpdateServiceAsync(int serviceId,
            string name,
            string description,
            decimal price,
            DateTime startDate,
            DateTime endDate,
            string updatedBy)
        {
            return await _serviceRepository.UpdateServiceAsync(serviceId, name, description, price, startDate, endDate, updatedBy);
        }

        public async Task<Service> UpdateServiceStatus(int serviceId, string status, string updatedBy)
        {
            return await _serviceRepository.UpdateServiceStatus(serviceId, status, updatedBy);
        }

        public async Task DeleteServiceAsync(int serviceId)
        {
            await _serviceRepository.DeleteServiceAsync(serviceId);
        }

        public async Task<List<Service>> GetServicesByRoomIdAsync(int roomId)
        {
            return await _serviceRepository.GetServicesByRoomIdAsync(roomId);
        }
    }
}

