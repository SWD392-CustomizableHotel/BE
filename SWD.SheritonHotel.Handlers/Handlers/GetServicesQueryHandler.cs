using Entities;
using MediatR;
using SWD.SheritonHotel.Data.Repositories.Interfaces;
using SWD.SheritonHotel.Domain.Queries;
using SWD.SheritonHotel.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Handlers.Handlers
{
    public class GetServicesQueryHandler : IRequestHandler<GetServicesQuery, IEnumerable<Service>>
    {
        private readonly IManageService _manageService;

        public GetServicesQueryHandler(IManageService serviceService)
        {
            _manageService = serviceService;
        }

        public async Task<IEnumerable<Service>> Handle(GetServicesQuery request, CancellationToken cancellationToken)
        {
            return await _manageService.GetAllAsync();
        }
    }
}
