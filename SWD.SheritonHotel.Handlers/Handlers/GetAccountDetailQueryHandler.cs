using AutoMapper;
using Entities;
using MediatR;
using SWD.SheritonHotel.Data.Repositories.Interfaces;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.Queries;

namespace SWD.SheritonHotel.Handlers.Handlers;

public class GetAccountDetailQueryHandler : IRequestHandler<GetAccountDetailQuery, AccountDto>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IMapper _mapper;

    public GetAccountDetailQueryHandler(IAccountRepository accountRepository, IMapper mapper)
    {
        _accountRepository = accountRepository;
        _mapper = mapper;
    }

    public async Task<AccountDto> Handle(GetAccountDetailQuery request, CancellationToken cancellationToken)
    {
        var user = await _accountRepository.GetAccountByIdAsync(request.AccountId);
            
        if (user == null)
        {
            return null;
        }

        var accountDto = _mapper.Map<ApplicationUser, AccountDto>(user);
            
        return accountDto;
    }
}