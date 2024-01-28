using Claims.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;

namespace Claims.Controllers;

[ApiController]
[Route("[controller]")]
public class CoversController(CosmosClient cosmosClient, ICoversService coversService) : ControllerBase
{
    private readonly ICoversService _coversService = coversService;

    private readonly Container _container = cosmosClient?.GetContainer("ClaimDb", "Cover")
                     ?? throw new ArgumentNullException(nameof(cosmosClient));

    [HttpPost]
    public async Task<IActionResult> ComputePremiumAsync(DateOnly startDate, DateOnly endDate, CoverType coverType)
    {
        return Ok(await _coversService.ComputePremiumAsync(startDate, endDate, coverType));
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        var results = await _coversService.GetAsync();
        return Ok(results);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(string id)
    {
        try
        {
            var response = await _container.ReadItemAsync<Cover>(id, new(id));
            return Ok(response.Resource);
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<ActionResult> CreateAsync(Cover cover)
    {
        var newCover = _coversService.CreateAsync(cover);
        return Ok(newCover);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(string id)
    {
        await _coversService.DeleteAsync(id);
        return Ok();
    }
}