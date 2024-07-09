using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWD.SheritonHotel.Domain.DTO;

namespace SWD.SheritonHotel.Domain.Commands
{
    public class CreateRoomCommand : IRequest<ResponseDto<int>>
    {
        public string Type { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string RoomNumber { get; set; }
        public int HotelId { get; set; }
        public IFormFile ImageUpload { get; set; }
    }
}
