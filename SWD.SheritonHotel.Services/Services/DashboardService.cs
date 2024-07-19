using SWD.SheritonHotel.Data.Repositories.Interfaces;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.DTO.Amenity;
using SWD.SheritonHotel.Domain.DTO.Booking;
using SWD.SheritonHotel.Domain.DTO.Notification;
using SWD.SheritonHotel.Domain.DTO.Payment;
using SWD.SheritonHotel.Domain.DTO.Responses;
using SWD.SheritonHotel.Domain.DTO.Room;
using SWD.SheritonHotel.Domain.DTO.Service;
using SWD.SheritonHotel.Domain.Entities;
using SWD.SheritonHotel.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Services.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IPaymentRepository _paymentRepository;

        public DashboardService(
            IBookingRepository bookingRepository,
            IUserRepository userRepository,
            IRoomRepository roomRepository,
            IPaymentRepository paymentRepository)
        {
            _bookingRepository = bookingRepository;
            _userRepository = userRepository;
            _roomRepository = roomRepository;
            _paymentRepository = paymentRepository;
        }

        public async Task<DashboardDto> GetDashboardDataAsync()
        {
            var bookings = await _bookingRepository.GetAll();
            var users = await _userRepository.GetAllUsers();
            var rooms = await _roomRepository.GetAll();
            var payments = await _paymentRepository.GetAll();

            var succeededPayments = payments.Where(p => p.Status == "Success").ToList();
            var todayPayments = succeededPayments.Where(p => p.CreatedDate.Date == DateTime.UtcNow.Date).ToList();
            var yesterdayPayments = succeededPayments.Where(p => p.CreatedDate.Date == DateTime.UtcNow.AddDays(-1).Date).ToList();

            var revenue = succeededPayments.Sum(p => p.Amount);
            var revenueIncreasePercentage = CalculateRevenueIncrease(succeededPayments);

            var monthlyRevenue = CalculateMonthlyRevenue(succeededPayments);

            var recentBookings = bookings.OrderByDescending(b => b.CreatedDate).Take(10).Select(b => new BookingHistoryDto
            {
                BookingId = b.Id,
                RoomType = b.Room.Type,
                RoomDescription = b.Room.Description,
                Rating = b.Rating,
                UserName = b.User.UserName,
                StartDate = b.StartDate ?? default,
                EndDate = b.EndDate ?? default,
                Services = b.BookingServices.Select(bs => new ServiceDto
                {
                    Id = bs.ServiceId,
                    Name = bs.Service.Name,
                    Description = bs.Service.Description,
                    Price = bs.Service.Price
                }).ToList(),
                Amenities = b.BookingAmenities.Select(ba => new AmenityDTO
                {
                    Id = ba.AmenityId,
                    Name = ba.Amenity.Name,
                    Description = ba.Amenity.Description,
                    Price = ba.Amenity.Price
                }).ToList(),
                Payments = b.Payments.Select(p => new PaymentDto
                {
                    Id = p.Id,
                    Amount = p.Amount,
                    Status = p.Status,
                    PaymentDate = p.CreatedDate
                }).ToList()
            }).ToList();

            var bestBookingRooms = rooms.OrderByDescending(r => r.Bookings.Count).Take(5).Select(r => new RoomDto
            {
                RoomId = r.Id,
                RoomNumber = r.Code,
                RoomType = r.Type,
                RoomDescription = r.Description,
                RoomStatus = r.Status.ToString(),
                RoomPrice = r.Price,
                IsDeleted = r.IsDeleted,
                Image = r.Image,
                NumberOfPeople = r.NumberOfPeople
            }).ToList();

            var todayNotifications = todayPayments.Select(p => new NotificationDto
            {
                UserName = p.Booking.User.UserName,
                RoomType = p.Booking.Room.Type,
                Amount = p.Amount,
                BookingDate = p.CreatedDate
            }).ToList();

            var yesterdayNotifications = yesterdayPayments.Select(p => new NotificationDto
            {
                UserName = p.Booking.User.UserName,
                RoomType = p.Booking.Room.Type,
                Amount = p.Amount,
                BookingDate = p.CreatedDate
            }).ToList();

            return new DashboardDto
            {
                OrdersCount = bookings.Count(b => b.Payments.Any(p => p.Status == "Success")),
                NewOrdersCount = todayPayments.Count,
                Revenue = revenue,
                RevenueIncreasePercentage = (double)revenueIncreasePercentage,
                CustomersCount = users.Count(),
                NewCustomersCount = users.Count(u => u.CreatedDate == DateTime.UtcNow.Date),
                RoomsCount = rooms.Count(),
                AvailableRoomsCount = rooms.Count(r => r.Status == "Available"),
                RecentBookings = recentBookings,
                BestBookingRooms = bestBookingRooms,
                TodayNotifications = todayNotifications,
                YesterdayNotifications = yesterdayNotifications,
                MonthlyRevenue = monthlyRevenue
            };
        }

        private double CalculateRevenueIncrease(IEnumerable<Payment> payments)
        {
            var previousWeekPayments = payments.Where(p => p.CreatedDate >= DateTime.UtcNow.AddDays(-14) && p.CreatedDate < DateTime.UtcNow.AddDays(-7)).Sum(p => p.Amount);
            var lastWeekPayments = payments.Where(p => p.CreatedDate >= DateTime.UtcNow.AddDays(-7) && p.CreatedDate < DateTime.UtcNow).Sum(p => p.Amount);

            if (previousWeekPayments == 0) return 100;
            return ((double)(lastWeekPayments - previousWeekPayments) / (double)previousWeekPayments) * 100;
        }

        private IEnumerable<decimal> CalculateMonthlyRevenue(IEnumerable<Payment> payments)
        {
            var currentYear = DateTime.UtcNow.Year;
            var monthlyRevenue = new decimal[12];

            for (int month = 1; month <= 12; month++)
            {
                monthlyRevenue[month - 1] = payments
                    .Where(p => p.CreatedDate.Year == currentYear && p.CreatedDate.Month == month)
                    .Sum(p => p.Amount);
            }

            return monthlyRevenue;
        }
    }
}
