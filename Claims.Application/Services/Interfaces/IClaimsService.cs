using Claims.Application.Dtos;
using Claims.Application.Models;

namespace Claims.Application.Services.Interfaces;

public interface IClaimsServices
{
    Task<IEnumerable<Claim>> GetAsync();

    Task<Claim> GetAsync(string id);

    Task<Claim> CreateAsync(ClaimDto claimDto);

    Task DeleteAsync(string id);
}