using Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using OtherObjects;
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
	public class DeleteServiceCommandHandler : IRequestHandler<DeleteServiceCommand, ResponseDto<bool>>
    {
        private readonly IServiceRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeleteServiceCommandHandler(IServiceRepository repository, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseDto<bool>> Handle(DeleteServiceCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            if (user == null || !(await _userManager.IsInRoleAsync(user, StaticUserRoles.ADMIN)))
            {
                return new ResponseDto<bool>
                {
                    IsSucceeded = false,
                    Message = "Unauthorized",
                    Errors = new[] { "You must be an admin to perform this operation." }
                };
            }

            try
            {
                var service = await _repository.GetByIdAsync(request.ServiceId);
                if (service == null)
                {
                    return new ResponseDto<bool>
                    {
                        IsSucceeded = false,
                        Message = "Service not found",
                        Errors = new[] { "No service found with the given ID." }
                    };
                }

                await _repository.DeleteAsync(service.Id);
                return new ResponseDto<bool>(true)
                {
                    Message = "Service deleted successfully"
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<bool>
                {
                    IsSucceeded = false,
                    Message = "An error occurred while deleting the service.",
                    Errors = new[] { ex.Message }
                };
            }
        }
    }
}
