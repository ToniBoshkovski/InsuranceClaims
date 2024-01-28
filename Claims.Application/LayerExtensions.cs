using Claims.Application.Services;
using Claims.Application.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Claims.Application;

public static class LayerExtensions
{
    public static void AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IClaimsServices, ClaimsServices>();
        services.AddScoped<ICoversService, CoversService>();
    }
}