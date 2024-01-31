using Claims.Application.Dtos;
using Claims.Application.Interfaces;
using Claims.Application.Models;
using Claims.Application.Services.Interfaces;
using Hangfire;

namespace Claims.Application.Services;

public class ClaimsServices(ICosmosDbService cosmosDbService, IAuditer auditer, IBackgroundJobClient jobClient) : IClaimsServices
{
    private readonly ICosmosDbService _cosmosDbService = cosmosDbService;
    private readonly IAuditer _auditer = auditer;
    private readonly IBackgroundJobClient _jobClient = jobClient;

    public async Task<IEnumerable<Claim>> GetAsync() => await _cosmosDbService.GetAsync<Claim>();

    public async Task<Claim> GetAsync(string id) => await _cosmosDbService.GetAsync<Claim>(id);

    public async Task<Claim> CreateAsync(ClaimDto claimDto)
    {
        Claim claim = Claim.MapToClaim(claimDto);
        claim.Id = Guid.NewGuid().ToString();
        claim.ItemType = "Claim";
        await _cosmosDbService.AddItemAsync(claim);

        _jobClient.Enqueue(() => _auditer.AuditClaim(claim.Id, "POST"));

        return claim;
    }

    public async Task DeleteAsync(string id)
    {
        await _cosmosDbService.DeleteItemAsync<Claim>(id);
        _jobClient.Enqueue(() => _auditer.AuditClaim(id, "DELETE"));
    }
}