using Claims.Application.Interfaces;
using Claims.Application.Services.Interfaces;

namespace Claims.Application.Services;

public class ClaimsServices : IClaimsServices
{
    private readonly ICosmosDbService _cosmosDbService;
    private readonly IAuditer _auditer;

    public ClaimsServices(ICosmosDbService cosmosDbService, IAuditer auditer)
    {
        _cosmosDbService = cosmosDbService;
        _auditer = auditer;
    }

    public async Task<IEnumerable<Claim>> GetAsync()
    {
        return await _cosmosDbService.GetAsync<Claim>();
    }

    public Task<Claim> GetAsync(string id)
    {
        return _cosmosDbService.GetAsync<Claim>(id);
    }

    public async Task<Claim> CreateAsync(Claim claim)
    {
        claim.Id = Guid.NewGuid().ToString();
        await _cosmosDbService.AddItemAsync(claim);
        _auditer.AuditClaim(claim.Id, "POST");
        return claim;
    }

    public Task DeleteAsync(string id)
    {
        _auditer.AuditClaim(id, "DELETE");
        return _cosmosDbService.DeleteItemAsync<Claim>(id);
    }
}