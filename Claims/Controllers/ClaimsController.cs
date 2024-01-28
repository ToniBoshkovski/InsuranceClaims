using Claims.API.Exceptions;
using Claims.Application.Services.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace Claims.Controllers;

[ApiController]
[Route("[controller]")]
public class ClaimsController(IClaimsServices claimsServices, IValidator<Claim> validator) : ControllerBase
{
    private readonly IClaimsServices _claimsServices = claimsServices;
    private readonly IValidator<Claim> _validator = validator;

    [HttpGet]
    public async Task<IActionResult> GetAsync()
        => Ok(await _claimsServices.GetAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(string id)
        => Ok(await _claimsServices.GetAsync(id));

    [HttpPost]
    public async Task<IActionResult> CreateAsync(Claim claim)
    {
        ValidationResult result = await _validator.ValidateAsync(claim);
        if (!result.IsValid) return BadRequest(result.CreateValidationErrorsResponse());

        return Ok(await _claimsServices.CreateAsync(claim));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(string id)
    {
        await _claimsServices.DeleteAsync(id);
        return Ok();
    }
}