using Claims.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Claims.Controllers;

/// <summary>
/// Covers Controller
/// </summary>
[ApiController]
[Route("[controller]")]
public class CoversController(ICoversService coversService) : ControllerBase
{
    private readonly ICoversService _coversService = coversService;

    /// <summary>
    /// Compute Premium For Covers
    /// </summary>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <param name="coverType"></param>
    /// <returns></returns>
    [HttpGet]
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
    /// <param name="cover"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> CreateAsync(Cover cover)
        => Ok(await _coversService.CreateAsync(cover));

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