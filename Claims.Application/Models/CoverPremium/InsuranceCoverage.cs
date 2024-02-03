namespace Claims.Application.Models.CoverPremium;

public abstract class InsuranceCoverage
{
    public abstract decimal Multiplier { get; }

    public abstract decimal GetDiscount(int insuranceDuration);
}