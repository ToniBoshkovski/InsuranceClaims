using Claims.Application.Interfaces;
using FluentValidation;

namespace Claims.Application.Validators;

public class ClaimValidator : AbstractValidator<Claim>
{
    private readonly ICosmosDbService _cosmosDbService;

    public ClaimValidator(ICosmosDbService cosmosDbService)
    {
        _cosmosDbService = cosmosDbService;

        RuleFor(x => x.DamageCost)
            .LessThanOrEqualTo(99999);

        RuleFor(x => x.Created)
            .MustAsync(ExistsWithinDateRange);
    }

    private async Task<bool> ExistsWithinDateRange(Claim model, DateTime value, CancellationToken cancellationToken)
    {
        DateOnly dateValue = DateOnly.FromDateTime(value);
        Cover cover = await _cosmosDbService.GetAsync<Cover>(model.CoverId);

        return dateValue >= cover?.StartDate && dateValue <= cover?.EndDate;
    }
}