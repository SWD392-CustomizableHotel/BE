using SWD.SheritonHotel.Domain.DTO.Booking;
using SWD.SheritonHotel.Domain.DTO.Notification;
using SWD.SheritonHotel.Domain.DTO.Room;

namespace SWD.SheritonHotel.Domain.DTO.Responses
{
    public class DashboardDto
    {
        public int OrdersCount { get; set; }
        public int NewOrdersCount { get; set; }
        public decimal Revenue { get; set; }
        public double RevenueIncreasePercentage { get; set; }
        public int CustomersCount { get; set; }
        public int NewCustomersCount { get; set; }
        public int RoomsCount { get; set; }
        public int AvailableRoomsCount { get; set; }
        public IEnumerable<BookingHistoryDto> RecentBookings { get; set; } = new List<BookingHistoryDto>();
        public IEnumerable<RoomDto> BestBookingRooms { get; set; } = new List<RoomDto>();
        public IEnumerable<NotificationDto> TodayNotifications { get; set; } = new List<NotificationDto>();
        public IEnumerable<NotificationDto> YesterdayNotifications { get; set; } = new List<NotificationDto>();
        public IEnumerable<decimal> MonthlyRevenue { get; set; } = new List<decimal>();
    }
}
