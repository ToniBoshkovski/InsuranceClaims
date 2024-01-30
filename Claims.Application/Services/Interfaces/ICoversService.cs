using Claims.Application.Dtos;
using Claims.Application.Enums;
using Claims.Application.Models;

namespace Claims.Application.Services.Interfaces;

public interface ICoversService
{
    decimal ComputePremium(DateOnly startDate, DateOnly endDate, CoverType coverType);

    Task<IEnumerable<Cover>> GetAsync();

    Task<Cover> GetAsync(string id);

    Task<Cover> CreateAsync(CoverDto coverDto);

    Task DeleteAsync(string id);
}