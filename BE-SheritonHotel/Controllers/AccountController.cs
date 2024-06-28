using System.Linq;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWD.SheritonHotel.Domain.Commands;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.OtherObjects;
using SWD.SheritonHotel.Domain.Queries;


namespace SWD.SheritonHotel.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "ADMIN")]
public class AccountController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccountController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAccounts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] AccountFilter accountFilter = null, [FromQuery] string? searchTerm = null)
    {
        try
        {
            var paginationFilter = new PaginationFilter(pageNumber, pageSize);
            var query = new GetAllAccountsQuery(paginationFilter, accountFilter, searchTerm);
            var account = await _mediator.Send(query);
            return Ok(account);
        }
        catch (Exception e)
        {
            return Ok(new BaseResponse<AccountDto> { IsSucceed = false, Result = null, Message = "No account found!" });
        }
    }
    
    [HttpGet("{accountId}")]
    public async Task<IActionResult> GetAccountDetail(string accountId)
    {
        try
        {
            var query = new GetAccountDetailQuery(accountId);
            var accountDetail = await _mediator.Send(query);
            return Ok(accountDetail);
        }
        catch (Exception e)
        {
            return Ok(new BaseResponse<AccountDto> { IsSucceed = false, Result = null, Message = "Account not found!" });
        }
    }
    
    [HttpPut("{accountId}")]
    public async Task<IActionResult> UpdateAccount(string accountId, [FromBody] AccountDto? accountDto)
    {
        var command = new UpdateAccountDetailCommand(accountId, accountDto);
        var result = await _mediator.Send(command);
        return Ok(new BaseResponse<AccountDto>
        {
            IsSucceed = result.IsSucceeded,
            Result = result.Data,
            Message = result.IsSucceeded ? "Account updated successfully" : "Failed to update account"
        });
    }
    
    [HttpDelete("{accountId}")]
    public async Task<IActionResult> DeleteAccount(string accountId)
    {
        var command = new DeleteAccountCommand
        {
            AccountId = accountId
        };
        var result = await _mediator.Send(command);
        return Ok(new BaseResponse<string>
        {
            IsSucceed = result.IsSucceeded,
            Result = result.IsSucceeded ? "Account deleted successfully!" : null,
            Message = result.IsSucceeded ? "Account deleted successfully!" : "Failed to delete account!"
        });

    }
}