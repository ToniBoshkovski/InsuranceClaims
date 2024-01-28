namespace Claims.Application.Services.Interfaces;

public interface ICoversService
{
    Task<decimal> ComputePremiumAsync(DateOnly startDate, DateOnly endDate, CoverType coverType);

    Task<IEnumerable<Cover>> GetAsync();

    Task<Cover> GetAsync(string id);

    Task<Cover> CreateAsync(Cover cover);

    Task DeleteAsync(string id);
}