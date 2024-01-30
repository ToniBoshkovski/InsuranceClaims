using Claims.Application.Models;

namespace Claims.Application.Interfaces;

public interface ICosmosDbService
{
    Task<IEnumerable<T>> GetAsync<T>();

    Task<T?> GetAsync<T>(string id);

    Task AddItemAsync(Claim item);

    Task AddItemAsync(Cover item);

    Task DeleteItemAsync<T>(string id);
}