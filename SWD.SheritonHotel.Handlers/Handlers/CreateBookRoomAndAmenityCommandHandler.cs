using Entities;
using Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SWD.SheritonHotel.Domain.Commands;
using SWD.SheritonHotel.Domain.OtherObjects;
using SWD.SheritonHotel.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Handlers.Handlers
{
    public class CreateBookRoomAndAmenityCommandHandler : IRequestHandler<CreateBookRoomAndAmenityCommand, string>
    {
        private readonly IRoomService _roomService;
        private readonly IAmenityService _amenityService;
        private readonly IBookingService _bookingService;
        private readonly IUserService _userService;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CreateBookRoomAndAmenityCommandHandler(IRoomService roomService, IAmenityService amenityService, IBookingService bookingService, IUserService userService, IBlobStorageService blobStorageService, IHttpContextAccessor httpContextAccessor)
        {
            _roomService = roomService;
            _userService = userService;
            _blobStorageService = blobStorageService;
            _amenityService = amenityService;
            _bookingService = bookingService;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<string> Handle(CreateBookRoomAndAmenityCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var room = await _roomService.GetRoomByIdAsync(request.RoomId);
                if (room == null || room.Status != "Available")
                {
                    return null;
                }

                var amenity = await _amenityService.GetAmenityByIdAsync(request.AmenityId);
                if (amenity == null || (amenity.Status != AmenityStatus.Normal && amenity.Status != AmenityStatus.Old))
                {
                    return null;
                }

                var user = await _userService.GetUserAsync();

                // Create booking
                var booking = new Booking
                {
                    RoomId = room.Id,
                    UserId = user.Id,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    Code = request.BookingCode,
                    CreatedBy = user.Email,
                    LastUpdatedBy = user.Email,
                    IsDeleted = false,
                    CreatedDate = DateTime.Now,
                    Rating = 5
                };

                // Tăng số lượng dùng amenity lên
                amenity.InUse++;
                await _amenityService.UpdateAmenityAsync(
                    amenity.Id,
                    amenity.Name,
                    amenity.Description,
                    amenity.Price,
                    amenity.Capacity,
                    amenity.InUse,
                    "System"
                 );

                var bookingId = await _bookingService.CreateBookingAsync(booking);

                // Create booking amenity
                var bookingAmenity = new BookingAmenity
                {
                    BookingId = bookingId,
                    AmenityId = amenity.Id,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    Code = request.AmenityBookingCode,
                    CreatedBy = user.Email,
                    LastUpdatedBy = user.Email,
                    IsDeleted = false,
                    CreatedDate = DateTime.Now
                };

                await _bookingService.CreateBookingAmenityAsync(bookingAmenity);
                return bookingId.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
