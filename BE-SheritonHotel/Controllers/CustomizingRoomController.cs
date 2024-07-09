using Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.Queries;

namespace SWD.SheritonHotel.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomizingRoomController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CustomizingRoomController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /* 
         * Lấy toàn bộ phòng trống có thể type Customizable và đang không sử dụng và không bị xóa
         * @params roomSize (string), numberOfPeople (int)
         * @return base response
         */
        [HttpPost]
        [Route("get-all")]
        public async Task<IActionResult> GetAllRooms([FromBody] GetAllCustomizableRoomsQuery query)
        {
            var results = await _mediator.Send(query);
            return Ok(new BaseResponse<Room>
            {
                Results = results,
                IsSucceed = true,
                Message = "Successfully get all customizable rooms"
            });
        }


        /*
         * Update lại phòng trống có khách đã đặt
         * Tính toán số tiền mà amenities, services đã chọn
        */


        // Check phòng ID có đang trống không
        [HttpGet]
        [Route("check-room/{roomId}")]
        public async Task<IActionResult> CheckRoomStatus()
        {
            return Ok(new BaseResponse<string>
            {
                IsSucceed = true,
                Message = "false"
            });
        }
    }
}
