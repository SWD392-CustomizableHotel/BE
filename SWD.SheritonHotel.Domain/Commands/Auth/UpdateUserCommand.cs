using System;
using MediatR;
using Microsoft.AspNetCore.Http;
using SWD.SheritonHotel.Domain.DTO.Responses;
using SWD.SheritonHotel.Domain.Entities;

namespace SWD.SheritonHotel.Domain.Commands.Auth
{
    public class UpdateUserCommand : IRequest<BaseResponse<ApplicationUser>>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? Dob { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public IFormFile? Certificate { get; set; }
    }
}
