namespace Claims.Application.Models.CoverPremium;

public class YachtCover : InsuranceCoverage
{
    public override decimal Multiplier => 1.1m;

    public override decimal GetDiscount(int insuranceDuration)
    {
        return insuranceDuration switch
        {
            < 180 => 0.05m,
            < 365 => 0.03m,
            _ => 0m
        };
    }
}