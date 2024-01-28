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
    public async Task<IEnumerable<Claim>> GetAsync()
    {
        return await _claimsServices.GetAsync();
    }

    [HttpGet("{id}")]
    public Task<Claim> GetAsync(string id)
    {
        return _claimsServices.GetAsync(id);
    }

    [HttpPost]
    public async Task<ActionResult> CreateAsync(Claim claim)
    {
        ValidationResult result = await _validator.ValidateAsync(claim);
        if (!result.IsValid) return BadRequest(result.CreateValidationErrorsResponse());

        var newClaim = _claimsServices.CreateAsync(claim);
        return Ok(newClaim);
    }

    [HttpDelete("{id}")]
    public Task DeleteAsync(string id)
    {
        return _claimsServices.DeleteAsync(id);
    }
}