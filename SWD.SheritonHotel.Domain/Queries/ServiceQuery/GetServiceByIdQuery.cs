using MediatR;
using SWD.SheritonHotel.Domain.DTO.Responses;
using SWD.SheritonHotel.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Domain.Queries.ServiceQuery
{
    public class GetServiceByIdQuery : IRequest<ResponseDto<Service>>
    {
        public int ServiceId { get; set; }

        public GetServiceByIdQuery(int serviceId)
        {
            ServiceId = serviceId;
        }
    }
}
