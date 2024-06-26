using Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using OtherObjects;
using SWD.SheritonHotel.Data.Repositories;
using SWD.SheritonHotel.Data.Repositories.Interfaces;
using SWD.SheritonHotel.Domain.Commands;
using SWD.SheritonHotel.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Handlers.Handlers
{
    public class CreateServiceCommandHandler : IRequestHandler<CreateServiceCommand, ResponseDto<Service>>
    {
        private readonly IServiceRepository _repository;
        private readonly IHotelRepository _hotelRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateServiceCommandHandler(IServiceRepository repository, IHotelRepository hotelRepository, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _userManager = userManager;
            _hotelRepository = hotelRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseDto<Service>> Handle(CreateServiceCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            if (user == null || !(await _userManager.IsInRoleAsync(user, StaticUserRoles.ADMIN)))
            {
                return new ResponseDto<Service>
                {
                    IsSucceeded = false,
                    Message = "Unauthorized",
                    Errors = new[] { "You must be an admin to perform this operation." }
                };
            }

            var hotelExists = await _hotelRepository.GetByIdAsync(request.HotelId);
            if (hotelExists == null)
            {
                return new ResponseDto<Service>
                {
                    IsSucceeded = false,
                    Message = "Invalid HotelId",
                    Errors = new[] { "The provided HotelId does not exist." }
                };
            }

            try
            {
                var code = GenerateServiceCode();
                var service = new Service
                {
                    Name = request.Name,
                    Price = request.Price,
                    Description = request.Description,
                    Status = "Closed", // default status
                    Code = code,
                    CreatedBy = user.UserName,
                    CreatedDate = DateTime.UtcNow,
                    LastUpdatedBy = user.UserName, 
                    LastUpdatedDate = DateTime.UtcNow,
                    HotelId = request.HotelId,
                    IsDeleted = false
                };

                await _repository.AddAsync(service);
                return new ResponseDto<Service>(service)
                {
                    Message = "Service created successfully"
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<Service>
                {
                    IsSucceeded = false,
                    Message = "An error occurred while creating the service.",
                    Errors = new[] { ex.Message }
                };
            }
        }

        private string GenerateServiceCode()
        {
            return Guid.NewGuid().ToString().Replace("-", "").Substring(0, 6).ToUpper(); 
        }
    }
}
