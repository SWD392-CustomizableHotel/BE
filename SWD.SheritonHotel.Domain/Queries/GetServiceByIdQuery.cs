using Entities;
using MediatR;
using SWD.SheritonHotel.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Domain.Queries
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
