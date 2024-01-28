using Claims.Application.Interfaces;
using Claims.Infrastructure.Auditing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Claims.Infrastructure;

public static class LayerExtensions
{
    public static void AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ICosmosDbService, ICosmosDbService>();
        services.AddScoped<IAuditer, Auditer>();
    }
}