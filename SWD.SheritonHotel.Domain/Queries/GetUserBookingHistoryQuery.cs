using Entities;
using MediatR;
using SWD.SheritonHotel.Domain.DTO;

namespace SWD.SheritonHotel.Domain.Queries;

public class GetUserBookingHistoryQuery : IRequest<ResponseDto<IEnumerable<Booking>>>
{
    
}