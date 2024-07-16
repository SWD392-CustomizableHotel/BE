namespace SWD.SheritonHotel.Domain.DTO
{
    public class RoomDto
    {
        public int RoomId { get; set; }
        public string RoomNumber { get; set; }
        public string RoomType { get; set; }
        public string RoomDescription { get; set; }
        public string RoomStatus { get; set; }
        public decimal RoomPrice { get; set; }
        public bool IsDeleted { get; set; }
        public string ImagePath { get; set; }
        public string? Image { get; set; }
        public int NumberOfPeople { get; set; }
    }
}