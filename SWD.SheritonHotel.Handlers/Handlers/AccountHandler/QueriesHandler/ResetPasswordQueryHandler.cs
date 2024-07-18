using AutoMapper;
using Interfaces;
using MediatR;
using SWD.SheritonHotel.Domain.DTO.Responses;
using SWD.SheritonHotel.Domain.Entities;
using SWD.SheritonHotel.Domain.Queries.AccountQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Handlers.Handlers.AccountHandler.QueriesHandler
{
    public class ResetPasswordQueryHandler : IRequestHandler<ResetPasswordQuery, BaseResponse<ApplicationUser>>
    {
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        public ResetPasswordQueryHandler(IAuthService authService, IMapper mapper)
        {
            _authService = authService;
            _mapper = mapper;
        }
        public async Task<BaseResponse<ApplicationUser>> Handle(ResetPasswordQuery request, CancellationToken cancellationToken)
        {
            return await _authService.ResetPassword(request.Email, request.Token, request.Password);
        }
    }
}
