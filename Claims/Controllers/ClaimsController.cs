using Claims.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Claims.Controllers;

[ApiController]
[Route("[controller]")]
public class ClaimsController(IClaimsServices claimsServices) : ControllerBase
{
    private readonly IClaimsServices _claimsServices = claimsServices;

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
        var newClaim = _claimsServices.CreateAsync(claim);
        return Ok(newClaim);
    }

    [HttpDelete("{id}")]
    public Task DeleteAsync(string id)
    {
        return _claimsServices.DeleteAsync(id);
    }
}