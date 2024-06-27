using AutoMapper;
using Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SWD.SheritonHotel.Data.Repositories.Interfaces;
using SWD.SheritonHotel.Domain.Commands;
using SWD.SheritonHotel.Domain.DTO;

namespace SWD.SheritonHotel.Handlers.Handlers;

public class UpdateAccountDetailCommandHandler : IRequestHandler<UpdateAccountDetailCommand, ResponseDto<AccountDto>>
{
    private readonly IAccountRepository _accountRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public UpdateAccountDetailCommandHandler(IAccountRepository accountRepository, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<ResponseDto<AccountDto>> Handle(UpdateAccountDetailCommand request, CancellationToken cancellationToken)
        {

            try
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

                user.FirstName = request.AccountDto.FirstName;
                user.LastName = request.AccountDto.LastName;
                user.UserName = request.AccountDto.UserName;
                user.Email = request.AccountDto.Email;
                user.PhoneNumber = request.AccountDto.PhoneNumber;
                user.Dob = request.AccountDto.Dob;

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    return new ResponseDto<AccountDto>
                    {
                        IsSucceeded = false,
                        Message = "Failed to update account",
                        Errors = result.Errors.Select(e => e.Description).ToArray()
                    };
                }

                var updatedAccountDto = _mapper.Map<ApplicationUser, AccountDto>(user);

                return new ResponseDto<AccountDto>
                {
                    IsSucceeded = true,
                    Message = "Account details updated successfully",
                    Data = updatedAccountDto
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<AccountDto>
                {
                    IsSucceeded = false,
                    Message = "An error occurred while updating the account details.",
                    Errors = new[] { ex.Message }
                };
            }
        }
    }
