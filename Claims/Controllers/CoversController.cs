using Claims.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Claims.Controllers;

[ApiController]
[Route("[controller]")]
public class CoversController(ICoversService coversService) : ControllerBase
{
    private readonly ICoversService _coversService = coversService;

    [HttpPost]
    public IActionResult ComputePremium(DateOnly startDate, DateOnly endDate, CoverType coverType)
        => Ok(_coversService.ComputePremium(startDate, endDate, coverType));

    [HttpGet]
    public async Task<IActionResult> GetAsync()
        => Ok(await _coversService.GetAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(string id)
        => Ok(await _coversService.GetAsync(id));

    [HttpPost]
    public async Task<IActionResult> CreateAsync(Cover cover)
        => Ok(await _coversService.CreateAsync(cover));

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(string id)
    {
        await _coversService.DeleteAsync(id);
        return Ok();
    }
}