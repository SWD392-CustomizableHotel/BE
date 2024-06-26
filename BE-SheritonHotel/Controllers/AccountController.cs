using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.OtherObjects;
using SWD.SheritonHotel.Domain.Queries;
using SWD.SheritonHotel.Services.Interfaces;

namespace SWD.SheritonHotel.API.Controllers;

[ApiController]
[Route("api/[controller]")]

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
        var account = await _accountService.GetAccountByIdAsync(accountId);
        if (account == null)
        {
            return NotFound($"Account with ID {accountId} not found.");
        }
        return Ok(account);
    }
    
    [HttpPut("{accountId}")]
    public async Task<IActionResult> UpdateAccount(string accountId, [FromBody] AccountDto accountDto)
    {
        if (accountDto == null || accountDto.Id != accountId)
        {
            return BadRequest("Invalid update data or ID mismatch.");
        }

        try
        {
            var updatedAccount = await _accountService.UpdateAccountAsync(accountId, accountDto);
            return Ok(updatedAccount);
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"Account with ID {accountId} not found.");
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpDelete("{accountId}")]
    public async Task<IActionResult> DeleteAccount(string accountId)
    {
        try
        {
            var result = await _accountService.SoftDeleteAccountAsync(accountId);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}