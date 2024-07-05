using Dtos;
using Entities;
using Interfaces;
using MediatR;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.Queries;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Handlers.Handlers
{
    public class VerifyEmailQueryHandler : IRequestHandler<VerifyEmailQuery, BaseResponse<object>>
    {
        private readonly IUserService _userService;

        public VerifyEmailQueryHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<BaseResponse<object>> Handle(VerifyEmailQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _userService.VerifyEmailTokenAsync(request.Email, request.Token);

                if (result)
                {
                    return new BaseResponse<object> { IsSucceed = true, Message = "Email verified successfully.", Result = null };
                }
                else
                {
                    return new BaseResponse<object> { IsSucceed = false, Message = "Invalid or expired token.", Result = null };
                }
            }
            catch (InvalidOperationException ex)
            {
                return new BaseResponse<object> { IsSucceed = false, Message = ex.Message, Result = null };
            }
        }
    }
}
