using MediatR;
using SWD.SheritonHotel.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Domain.Queries
{
    public class GetPaymentIdQuery : IRequest<ResponseDto<PaymentDto>>
    {
        public int BookingId { get; set; }
    }
}
