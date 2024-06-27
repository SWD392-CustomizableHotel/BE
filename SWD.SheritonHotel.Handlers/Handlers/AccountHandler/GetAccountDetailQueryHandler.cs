using AutoMapper;
using Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using SWD.SheritonHotel.Data.Repositories.Interfaces;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.Queries;

namespace SWD.SheritonHotel.Handlers.Handlers;

public class GetAccountDetailQueryHandler : IRequestHandler<GetAccountDetailQuery, ResponseDto<AccountDto>>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;

    public GetAccountDetailQueryHandler(IAccountRepository accountRepository, IMapper mapper, UserManager<ApplicationUser> userManager)
    {
        _accountRepository = accountRepository;
        _mapper = mapper;
        _userManager = userManager;
    }

    public async Task<ResponseDto<AccountDto>> Handle(GetAccountDetailQuery request, CancellationToken cancellationToken)
    {
        var user = await _accountRepository.GetAccountByIdAsync(request.AccountId);
            
        if (user == null)
        {
            return new ResponseDto<AccountDto>
            {
                IsSucceeded = false,
                Message = "Account not found",
                Errors = new[] { "No account found with the provided ID." }
            };
        }

        var accountDto = _mapper.Map<ApplicationUser, AccountDto>(user);
        var roles = await _userManager.GetRolesAsync(user);
        accountDto.Roles = roles.ToList();
            
        return new ResponseDto<AccountDto>
        {
            IsSucceeded = true,
            Message = "Account details fetched successfully",
            Data = accountDto
        };
    }
}