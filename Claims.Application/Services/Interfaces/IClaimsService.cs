namespace Claims.Application.Services.Interfaces;

public interface IClaimsServices
{
    Task<IEnumerable<Claim>> GetAsync();

    Task<Claim> GetAsync(string id);

    Task<Claim> CreateAsync(Claim claim);

    Task DeleteAsync(string id);
}