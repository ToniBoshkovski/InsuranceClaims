using Claims.Application.Services;
using Claims.Application.Services.Interfaces;
using Claims.Application.Validators;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Claims.Application;

public static class LayerExtensions
{
    public static void AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IClaimsServices, ClaimsServices>();
        services.AddScoped<ICoversService, CoversService>();

        services.AddScoped<IValidator<Claim>, ClaimValidator>();
        services.AddScoped<IValidator<Cover>, CoverValidator>();
    }
}