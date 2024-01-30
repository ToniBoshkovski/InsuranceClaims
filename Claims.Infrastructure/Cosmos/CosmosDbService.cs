using Claims.Application.Interfaces;
using Claims.Application.Models;
using Microsoft.Azure.Cosmos;

namespace Claims.Infrastructure.Cosmos;

public class CosmosDbService : ICosmosDbService
{
    private readonly Container _container;

    public CosmosDbService(CosmosClient dbClient,
        string databaseName,
        string containerName)
    {
        ArgumentNullException.ThrowIfNull(dbClient);
        _container = dbClient.GetContainer(databaseName, containerName);
    }

    public async Task<IEnumerable<T>> GetAsync<T>()
    {
        var query = _container.GetItemQueryIterator<T>(new QueryDefinition($"SELECT * FROM c WHERE c.itemType = '{typeof(T).Name}'"));
        var results = new List<T>();
        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync();
            results.AddRange(response.ToList());
        }
        return results;
    }

    public async Task<T?> GetAsync<T>(string id)
    {
        try
        {
            var response = await _container.ReadItemAsync<T>(id, new(id));
            return response.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return default;
        }
    }

    public async Task AddItemAsync(Claim item) => await _container.CreateItemAsync(item, new(item.Id));

    public async Task AddItemAsync(Cover item) => await _container.CreateItemAsync(item, new(item.Id));

    public async Task DeleteItemAsync<T>(string id) => await _container.DeleteItemAsync<T>(id, new(id));
}