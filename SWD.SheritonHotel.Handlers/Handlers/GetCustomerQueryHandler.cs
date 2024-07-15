using MediatR;
using SWD.SheritonHotel.Data.Repositories.Interfaces;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Handlers.Handlers
{
	public class GetCustomerQueryHandler : IRequestHandler<GetCustomerQuery, List<CustomerDTO>>
	{
		private readonly IUserRepository _userRepository;

		public GetCustomerQueryHandler(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		public async Task<List<CustomerDTO>> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
		{
			var customer = await _userRepository.GetCustomerByRoleAsync("CUSTOMER");
			return customer;
		}
	}
}
