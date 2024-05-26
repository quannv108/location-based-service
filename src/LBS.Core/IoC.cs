using LBS.Contracts;
using LBS.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LBS.Core;

public static class IoC
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddScoped<INamedLocationService, NamedLocationService>();
        return services;
    }
}