using System.ComponentModel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWD.SheritonHotel.Domain.Commands;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.OtherObjects;
using SWD.SheritonHotel.Domain.Queries;
using SWD.SheritonHotel.Services.Interfaces;

namespace SWD.SheritonHotel.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "ADMIN")]
public class AccountController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IAccountService _accountService;

    public AccountController(IMediator mediator, IAccountService accountService)
    {
        _mediator = mediator;
        _accountService = accountService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAccounts(
        [FromQuery] int pageNumber = 1, 
        [FromQuery] int pageSize = 10, 
        [FromQuery] AccountFilter accountFilter = null, 
        [FromQuery] string searchTerm = null)
    {
        var paginationFilter = new PaginationFilter(pageNumber, pageSize);
        var query = new GetAllAccountsQuery(paginationFilter, accountFilter, searchTerm);
        var account = await _mediator.Send(query);
        return Ok(account);
    }
    
    [HttpGet("{accountId}")]
    public async Task<IActionResult> GetAccountDetail(string accountId)
    {
        var query = new GetAccountDetailQuery(accountId);
        var accountDetail = await _mediator.Send(query);
        return Ok(accountDetail);
    }
    
    [HttpPut("{accountId}")]
    public async Task<IActionResult> UpdateAccount(string accountId, [FromBody] AccountDto accountDto)
    {
        var command = new UpdateAccountDetailCommand(accountId, accountDto);
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    
    [HttpDelete("{accountId}")]
    public async Task<IActionResult> DeleteAccount(string accountId)
    {
        var command = new DeleteAccountCommand
        {
            AccountId = accountId
        };
        var result = await _mediator.Send(command);
        return Ok(result);

    }
}