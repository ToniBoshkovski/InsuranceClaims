using Claims.Application.Enums;

namespace Claims.Application.Models.CoverPremium;

public static class PremiumCalculator
{
    public static decimal ComputePremium(DateOnly startDate, DateOnly endDate, CoverType coverType)
    {
        InsuranceCoverage cover = coverType switch
        {
            CoverType.Yacht => new YachtCover(),
            CoverType.PassengerShip => new PassengerShipCover(),
            CoverType.Tanker => new TankerCover(),
            CoverType.ContainerShip => new ContainerShipCover(),
            CoverType.BulkCarrier => new BulkCarrierCover(),
            _ => throw new NotImplementedException()
        };

        return PremiumFormula(startDate, endDate, cover);
    }

    private static decimal PremiumFormula(DateOnly startDate, DateOnly endDate, InsuranceCoverage cover)
    {
        var baseDayRate = 1250m;
        var premiumPerDay = baseDayRate * cover.Multiplier;
        var insuranceLength = endDate.DayNumber - startDate.DayNumber;
        var totalPremium = 0m;

        for (var i = 0; i < insuranceLength; i++)
        {
            totalPremium += premiumPerDay - premiumPerDay * cover.GetDiscount(i);
        }

        return totalPremium;
    }
}