using Claims.Application.Interfaces;
using Claims.Infrastructure.Auditing;
using Claims.Infrastructure.Cosmos;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Claims.Infrastructure;

public static class LayerExtensions
{
    public static void AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(sp =>
        {
            IConfigurationSection cosmosConfig = configuration.GetSection("CosmosDb");
            string account = cosmosConfig.GetSection("Account").Value ?? throw new KeyNotFoundException();
            string key = cosmosConfig.GetSection("Key").Value ?? throw new KeyNotFoundException();

            return new CosmosClient(account, key);
        });

        services.AddScoped<ICosmosDbService, CosmosDbService>(sp =>
        {
            IConfigurationSection cosmosConfig = configuration.GetSection("CosmosDb");
            string databaseName = cosmosConfig.GetSection("DatabaseName").Value ?? throw new KeyNotFoundException();
            string containerName = cosmosConfig.GetSection("ContainerName").Value ?? throw new KeyNotFoundException();

            var cosmosClient = sp.GetRequiredService<CosmosClient>();

            return new CosmosDbService(cosmosClient, databaseName, containerName);
        });

        services.AddScoped<IAuditer, Auditer>();
    }
}