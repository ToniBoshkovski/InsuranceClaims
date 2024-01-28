using Claims.Application.Interfaces;
using Microsoft.Azure.Cosmos;

namespace Claims.Infrastructure.Cosmos;

public class CosmosDbService : ICosmosDbService
{
    private readonly Container _container;

    public CosmosDbService(CosmosClient dbClient,
        string databaseName,
        string containerName)
    {
        if (dbClient == null) throw new ArgumentNullException(nameof(dbClient));
        _container = dbClient.GetContainer(databaseName, containerName);
    }

    public async Task<IEnumerable<T>> GetAsync<T>()
    {
        var query = _container.GetItemQueryIterator<T>(new QueryDefinition("SELECT * FROM c"));
        var results = new List<T>();
        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync();

            results.AddRange(response.ToList());
        }
        return results;
    }

    public async Task<T> GetAsync<T>(string id)
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

    public Task AddItemAsync(Claim item)
    {
        return _container.CreateItemAsync(item, new(item.Id));
    }

    public Task AddItemAsync(Cover item)
    {
        return _container.CreateItemAsync(item, new(item.Id));
    }

    public Task DeleteItemAsync<T>(string id)
    {
        return _container.DeleteItemAsync<T>(id, new(id));
    }
}