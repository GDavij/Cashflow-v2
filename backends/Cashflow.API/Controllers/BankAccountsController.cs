using Cashflow.Domain.Features.FinancialDistribution.CreateBankAccount;
using Cashflow.Domain.Features.FinancialDistribution.DeleteBankAccount;
using Cashflow.Domain.Features.FinancialDistribution.GetBankAccount;
using Cashflow.Domain.Features.FinancialDistribution.ListAccountTypes;
using Cashflow.Domain.Features.FinancialDistribution.ListBankAccounts;
using Cashflow.Domain.Features.FinancialDistribution.UpdateBankAccount;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace Cashflow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BankAccountsController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(CreateBankAccountHandler.Response), 201)]
    [ProducesResponseType(typeof(List<ValidationFailure>), 400)]
    public async Task<IActionResult> CreateBankAccount([FromBody] CreateBankAccountHandler.Request request,
        [FromServices] CreateBankAccountHandler handler,
        [FromServices] CreateBankAccountRequestValidator validator)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (validationResult.IsValid)
        {
            var bankAccount = await handler.HandleAsync(request);
            return Created($"bank-accounts/{bankAccount.Id}", bankAccount);
        }

        return BadRequest(validationResult.Errors);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(DeleteBankAccountHandler.Response), 202)]
    public async Task<IActionResult> DeleteBankAccount([FromRoute] long id,
        [FromServices] DeleteBankAccountHandler handler)
    {
        return Accepted(await handler.HandleAsync(id));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GetBankAccountHandler.Response), 200)]
    public async Task<IActionResult> GetBankAccount([FromRoute] long id,
        [FromServices] GetBankAccountHandler handler,
        CancellationToken cancellationToken)
    {
        return Ok(await handler.HandleAsync(id, cancellationToken));
    }

    [HttpGet]
    [ProducesResponseType(typeof(ListBankAccountsHandler.Response), 200)]
    public async Task<IActionResult> ListBankAccounts([FromQuery] ListBankAccountsHandler.Request request, 
                                                      [FromServices] ListBankAccountsHandler handler,
        CancellationToken cancellationToken)
    {
        return Ok(await handler.HandleAsync(request, cancellationToken));
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(UpdateBankAccountHandler.Response), 202)]
    [ProducesResponseType(typeof(List<ValidationResult>), 400)]
    public async Task<IActionResult> UpdateBankAccount([FromRoute] long id,
        [FromBody] UpdateBankAccountHandler.Request request,
        [FromServices] UpdateBankAccountHandler handler,
        [FromServices] UpdateBankAccountRequestValidator validator)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (validationResult.IsValid)
        {
            await handler.HandleAsync(id, request);
            return Accepted();
        }

        return BadRequest(validationResult.Errors);
    }

    [HttpGet("types")]
    [ProducesResponseType(typeof(ListAccountTypesHandler.Response), 200)]
    public async Task<IActionResult> GetAllAccountTypes([FromServices] ListAccountTypesHandler handler, CancellationToken cancellationToken)
    {
        return Ok(await handler.HandleAsync(cancellationToken));
    }
}