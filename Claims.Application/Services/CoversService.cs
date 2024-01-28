using Claims.Application.Interfaces;
using Claims.Application.Services.Interfaces;

namespace Claims.Application.Services;

public class CoversService : ICoversService
{
    private readonly ICosmosDbService _cosmosDbService;
    private readonly IAuditer _auditer;

    public CoversService(ICosmosDbService cosmosDbService, IAuditer auditer)
    {
        _cosmosDbService = cosmosDbService;
        _auditer = auditer;
    }

    public async Task<decimal> ComputePremiumAsync(DateOnly startDate, DateOnly endDate, CoverType coverType)
    {
        return ComputePremium(startDate, endDate, coverType);
    }

    public async Task<IEnumerable<Cover>> GetAsync()
    {
        return await _cosmosDbService.GetAsync<Cover>();
    }

    public async Task<Cover> GetAsync(string id)
    {
        return await _cosmosDbService.GetAsync<Cover>(id);
    }

    public async Task<Cover> CreateAsync(Cover cover)
    {
        cover.Id = Guid.NewGuid().ToString();
        cover.Premium = ComputePremium(cover.StartDate, cover.EndDate, cover.Type);
        await _cosmosDbService.AddItemAsync(cover);
        _auditer.AuditClaim(cover.Id, "POST");
        return cover;
    }

    public Task DeleteAsync(string id)
    {
        _auditer.AuditCover(id, "DELETE");
        return _cosmosDbService.DeleteItemAsync<Cover>(id);
    }

    private decimal ComputePremium(DateOnly startDate, DateOnly endDate, CoverType coverType)
    {
        var multiplier = 1.3m;
        if (coverType == CoverType.Yacht)
        {
            multiplier = 1.1m;
        }

        if (coverType == CoverType.PassengerShip)
        {
            multiplier = 1.2m;
        }

        if (coverType == CoverType.Tanker)
        {
            multiplier = 1.5m;
        }

        var premiumPerDay = 1250 * multiplier;
        var insuranceLength = endDate.DayNumber - startDate.DayNumber;
        var totalPremium = 0m;

        for (var i = 0; i < insuranceLength; i++)
        {
            if (i < 30) totalPremium += premiumPerDay;
            if (i < 180 && coverType == CoverType.Yacht) totalPremium += premiumPerDay - premiumPerDay * 0.05m;
            else if (i < 180) totalPremium += premiumPerDay - premiumPerDay * 0.02m;
            if (i < 365 && coverType != CoverType.Yacht) totalPremium += premiumPerDay - premiumPerDay * 0.03m;
            else if (i < 365) totalPremium += premiumPerDay - premiumPerDay * 0.08m;
        }

        return totalPremium;
    }
}