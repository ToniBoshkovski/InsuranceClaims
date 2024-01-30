using Claims.API.Exceptions;
using Claims.Application.Dtos;
using Claims.Application.Enums;
using Claims.Application.Services.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace Claims.Controllers;

/// <summary>
/// Covers Controller
/// </summary>
[ApiController]
[Route("[controller]")]
public class CoversController(ICoversService coversService, IValidator<CoverDto> validator) : ControllerBase
{
    private readonly ICoversService _coversService = coversService;
    private readonly IValidator<CoverDto> _validator = validator;

    /// <summary>
    /// Compute Premium For Covers
    /// </summary>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <param name="coverType"></param>
    /// <returns></returns>
    [HttpGet("computer-premium")]
    public IActionResult ComputePremium(DateOnly startDate, DateOnly endDate, CoverType coverType)
        => Ok(_coversService.ComputePremium(startDate, endDate, coverType));

    /// <summary>
    /// Get all covers
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAsync()
        => Ok(await _coversService.GetAsync());

    /// <summary>
    /// Get cover by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(string id)
        => Ok(await _coversService.GetAsync(id));

    /// <summary>
    /// Create new cover
    /// </summary>
    /// <param name="coverDto"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> CreateAsync(CoverDto coverDto)
    {
        ValidationResult result = await _validator.ValidateAsync(coverDto);
        if (!result.IsValid) return BadRequest(result.CreateValidationErrorsResponse());

        return Ok(await _coversService.CreateAsync(coverDto));
    }

    /// <summary>
    /// Delete cover
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(string id)
    {
        await _coversService.DeleteAsync(id);
        return Ok();
    }
}