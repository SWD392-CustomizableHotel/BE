using Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
    public class GetAllHotelsQueryHandler : IRequestHandler<GetAllHotelsQuery, ResponseDto<List<Hotel>>>
    {
        private readonly IHotelService _hotelService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public GetAllHotelsQueryHandler(IHotelService hotelService, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _hotelService = hotelService;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseDto<List<Hotel>>> Handle(GetAllHotelsQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            //Check User Admin 
            /*if (user == null || (await _userManager.IsInRoleAsync(user, "ADMIN")))
            {
                return new ResponseDto<List<Hotel>>
                {
                    Data = new List<Hotel>(),
                    IsSucceeded = false,
                    Message = "Unauthorized",
                    Errors = new[] { "You must be an customer to perform this operation." }
                };
            }*/
            var response = await _hotelService.GetAllHotelsAsync();
            return response;
        }
    }
}
