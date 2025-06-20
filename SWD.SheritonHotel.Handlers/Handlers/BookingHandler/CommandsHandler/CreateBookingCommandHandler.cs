﻿using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SWD.SheritonHotel.Domain.Commands.Booking;
using SWD.SheritonHotel.Domain.DTO.Responses;
using SWD.SheritonHotel.Domain.Entities;
using SWD.SheritonHotel.Services;
using SWD.SheritonHotel.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Handlers.Handlers.BookingHandler.CommandsHandler
{
    public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, ResponseDto<int>>
    {
        private readonly IBookingService _bookingService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateBookingCommandHandler(IBookingService bookingService, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _bookingService = bookingService;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ResponseDto<int>> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            var newBooking = new Booking
            {
                Code = request.Code,
                Rating = request.Rating,
                RoomId = request.RoomId,
                UserId = request.UserId,
                CreatedBy = user.UserName,
                LastUpdatedBy = user.UserName,
                StartDate = request.StartDate.ToLocalTime(),
                EndDate = request.EndDate.ToLocalTime(),
                CreatedDate = DateTime.UtcNow.ToLocalTime()
            };

            try
            {
                var newBookingId = await _bookingService.CreateBookingAsync(newBooking);
                return new ResponseDto<int>(newBookingId);
            }
            catch (Exception ex)
            {
                return new ResponseDto<int>
                {
                    IsSucceeded = false,
                    Message = "An error occurred while creating the booking.",
                    Errors = new[] { ex.Message }
                };
            }
        }
    }
}
