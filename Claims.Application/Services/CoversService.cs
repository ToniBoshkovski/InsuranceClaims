using Claims.Application.Dtos;
using Claims.Application.Enums;
using Claims.Application.Interfaces;
using Claims.Application.Models;
using Claims.Application.Services.Interfaces;
using System.Security.Claims;

namespace Claims.Application.Services;

public class CoversService(ICosmosDbService cosmosDbService, IAuditer auditer) : ICoversService
{
    private readonly ICosmosDbService _cosmosDbService = cosmosDbService;
    private readonly IAuditer _auditer = auditer;

    public decimal ComputePremium(DateOnly startDate, DateOnly endDate, CoverType coverType) => ComputePremiumInternal(startDate, endDate, coverType);

    public async Task<IEnumerable<Cover>> GetAsync() => await _cosmosDbService.GetAsync<Cover>();

    public async Task<Cover> GetAsync(string id) => await _cosmosDbService.GetAsync<Cover>(id);

    public async Task<Cover> CreateAsync(CoverDto coverDto)
    {
        Cover cover = Cover.MapToCover(coverDto);
        cover.Id = Guid.NewGuid().ToString();
        cover.ItemType = "Cover";
        cover.Premium = ComputePremiumInternal(cover.StartDate, cover.EndDate, cover.Type);
        await _cosmosDbService.AddItemAsync(cover);
        await _auditer.AuditClaim(cover.Id, "POST");
        return cover;
    }

    public async Task DeleteAsync(string id)
    {
        await _auditer.AuditCover(id, "DELETE");
        await _cosmosDbService.DeleteItemAsync<Cover>(id);
    }

    private static decimal ComputePremiumInternal(DateOnly startDate, DateOnly endDate, CoverType coverType)
    {
        var baseDayRate = 1250m;
        var multiplier = coverType switch
        {
            CoverType.Yacht => 1.1m,
            CoverType.PassengerShip => 1.2m,
            CoverType.Tanker => 1.5m,
            _ => 1.3m
        };

        var premiumPerDay = baseDayRate * multiplier;
        var insuranceLength = endDate.DayNumber - startDate.DayNumber;
        var totalPremium = 0m;

        for (var i = 0; i < insuranceLength; i++)
        {
            if (i < 30)
            {
                totalPremium += premiumPerDay;
            }
            else if (i < 180)
            {
                totalPremium += ApplyDiscount(premiumPerDay, coverType, 0.05m, 0.02m);
            }
            else if (i < 365)
            {
                totalPremium += ApplyDiscount(premiumPerDay, coverType, 0.03m, 0.01m);
            }
        }

        return totalPremium;
    }

    private static decimal ApplyDiscount(decimal premiumPerDay, CoverType type, decimal yachtDiscount, decimal othersDiscount)
        => premiumPerDay - premiumPerDay * (type == CoverType.Yacht ? yachtDiscount : othersDiscount);
}