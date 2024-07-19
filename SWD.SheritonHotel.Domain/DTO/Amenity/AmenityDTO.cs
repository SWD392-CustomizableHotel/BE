using SWD.SheritonHotel.Domain.OtherObjects;

namespace SWD.SheritonHotel.Domain.DTO.Amenity
{
    public class AmenityDTO
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public AmenityStatus Status { get; set; }
    }
}