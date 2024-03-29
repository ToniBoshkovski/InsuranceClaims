using Claims.API.Exceptions;
using Claims.Application.Dtos;
using Claims.Application.Services.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace Claims.Controllers;

/// <summary>
/// Claims Controller
/// </summary>
[ApiController]
[Route("[controller]")]
public class ClaimsController(IClaimsServices claimsServices, IValidator<ClaimDto> validator) : ControllerBase
{
    private readonly IClaimsServices _claimsServices = claimsServices;
    private readonly IValidator<ClaimDto> _validator = validator;

    /// <summary>
    /// Get all claims
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAsync()
        => Ok(await _claimsServices.GetAsync());

    /// <summary>
    /// Get claim by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(string id)
        => Ok(await _claimsServices.GetAsync(id));

    /// <summary>
    /// Create new claim
    /// </summary>
    /// <param name="claimDto"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> CreateAsync(ClaimDto claimDto)
    {
        ValidationResult result = await _validator.ValidateAsync(claimDto);
        if (!result.IsValid) return BadRequest(result.CreateValidationErrorsResponse());

        return Ok(await _claimsServices.CreateAsync(claimDto));
    }

    /// <summary>
    /// Delete claim
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(string id)
    {
        await _claimsServices.DeleteAsync(id);
        return Ok();
    }
}