using Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWD.SheritonHotel.Domain.Commands;
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
         * @params Amenity ID, Room ID
         * Tạo Booking: Room 
         * Tạo Amenity Booking
        */
        [HttpPost]
        [Route("booking")]
        public async Task<IActionResult> CreateBookRoomAndAmenity([FromBody] CreateBookRoomAndAmenityCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                if (result == null)
                {
                    return Ok(new BaseResponse<string>
                    {
                        IsSucceed = false,
                        Message = "Cannot create booking room and amenity",
                    });
                }
                return Ok(new BaseResponse<string>
                {
                    IsSucceed = true,
                    Result = result, // trả về bookingId
                    Message = "Create customization type booking successfully"
                });
            }
            catch (Exception ex)
            {
                return Ok(new BaseResponse<string>
                {
                    IsSucceed = false,
                    Message = "Error: " + ex.Message,
                });
            }
        }

        /*
         * Create payment intent riêng cho customizing
         * 
         */
        [HttpPost]
        [Route("create-payment-intent")]
        public async Task<IActionResult> CreatePaymentIntent([FromBody] CreatePaymentIntentCustomizableCommand command)
        {
            try
            {
                var results = await _mediator.Send(command);
                return Ok(new BaseResponse<string>
                {
                    Results = results,
                    IsSucceed = true,
                    Message = "Create payment intent successfully"
                });
            }
            catch (Exception ex)
            {
                return Ok(new BaseResponse<string>
                {
                    IsSucceed = false,
                    Message = "Error: " + ex.Message,
                });
            }
        }

        /*
         * Lấy amenities dựa trên Amenities Type (Basic, Advanced, Family)
         */
        [HttpGet]
        [Route("get-amenity/{type}")]
        public async Task<IActionResult> GetAmenityByType(string type)
        {
            var query = new GetAmenityByTypeQuery()
            {
                AmenityType = type
            };
            var results = await _mediator.Send(query);
            return Ok(new BaseResponse<Amenity>
            {
                IsSucceed = true,
                Results = results,
                Message = "Retrieved amenity successfully"
            });
        }

        /*
         * Upload ảnh blob lên azure và update vào entity Room
         * Update lại status room = "Booked"
         */
        [HttpPost]
        [Route("update-room")]
        public async Task<IActionResult> UploadCanvasAndUpdateStatusRoom([FromForm] IFormFile canvasImage, [FromForm] int roomId)
        {
            try
            {
                if (canvasImage == null || canvasImage.Length == 0)
                {
                    return Ok(new BaseResponse<string>
                    {
                        IsSucceed = false,
                        Message = "Invalid image file"
                    });
                }

                var command = new UploadCanvasAndUpdateRoomStatusCommand()
                {
                    RoomId = roomId,
                    canvasImage = canvasImage
                };
                var result = await _mediator.Send(command);
                return Ok(new BaseResponse<string>
                {
                    IsSucceed = true,
                    Message = "Room updated successfully",
                    Result = result
                });
            }
            catch (Exception ex)
            {
                return Ok(new BaseResponse<string>
                {
                    IsSucceed = false,
                    Message = "Error: " + ex.Message
                });
            }
        }
    }
}
