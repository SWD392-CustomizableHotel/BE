using AutoMapper;
using Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SWD.SheritonHotel.Data.Repositories.Interfaces;
using SWD.SheritonHotel.Domain.Commands;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.Entities;
using SWD.SheritonHotel.Services.Interfaces;

namespace SWD.SheritonHotel.Handlers.Handlers;

public class AssignServiceCommandHandler : IRequestHandler<AssignServiceCommand, ResponseDto<AssignedService>>
    {
        private readonly IAssignServiceRepository _assignServiceRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AssignServiceCommandHandler(IAssignServiceRepository assignServiceRepository, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _assignServiceRepository = assignServiceRepository;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseDto<AssignedService>> Handle(AssignServiceCommand request, CancellationToken cancellationToken)
        {

            try
            {
                var assignedService = new AssignedService
                {
                    UserId = request.UserId,
                    ServiceId = request.ServiceId
                };

                var result = await _assignServiceRepository.AssignServiceToStaff(assignedService);
                return new ResponseDto<AssignedService>
                {
                    IsSucceeded = true,
                    Message = "Service assigned successfully",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<AssignedService>
                {
                    IsSucceeded = false,
                    Message = "An error occurred while assigning the service.",
                    Errors = new[] { ex.Message }
                };
            }
        }
    }