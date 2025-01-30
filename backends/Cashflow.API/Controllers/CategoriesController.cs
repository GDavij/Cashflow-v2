using Cashflow.Domain.Features.FinancialBoundaries;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace Cashflow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(CreateCategoryHandler.Response), 201)]
    [ProducesResponseType(typeof(List<ValidationFailure>), 400)]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryHandler.Request request,
        [FromServices] CreateCategoryHandler handler,
        [FromServices] CreateCategoryRequestValidator validator)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (validationResult.IsValid)
        {
            var category = await handler.HandleAsync(request);
            return Created($"categories/{category.Id}", category);
        }

        return BadRequest(validationResult.Errors);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(DeleteCategoryHandler.Response), 202)]
    public async Task<IActionResult> DeleteCategory([FromRoute] long id,
        [FromServices] DeleteCategoryHandler handler)
    {
        return Accepted(await handler.HandleAsync(id));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GetCategoryHandler.Response), 200)]
    public async Task<IActionResult> GetCategory([FromRoute] long id,
        [FromServices] GetCategoryHandler handler,
        CancellationToken cancellationToken)
    {
        return Ok(await handler.HandleAsync(id, cancellationToken));
    }

    [HttpGet]
    [ProducesResponseType(typeof(ListCategoriesHandler.Response), 200)]
    public async Task<IActionResult> ListCategories([FromServices] ListCategoriesHandler handler,
        CancellationToken cancellationToken)
    {
        return Ok(await handler.HandleAsync(cancellationToken));
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(UpdateCategoryHandler.Response), 202)]
    [ProducesResponseType(typeof(List<ValidationResult>), 400)]
    public async Task<IActionResult> UpdateCategory([FromRoute] long id,
        [FromBody] UpdateCategoryHandler.Request request,
        [FromServices] UpdateCategoryHandler handler,
        [FromServices] UpdateCategoryRequestValidator validator)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (validationResult.IsValid)
        {
            return Ok(await handler.HandleAsync(id, request));
        }

        return BadRequest(validationResult.Errors);
    }
}