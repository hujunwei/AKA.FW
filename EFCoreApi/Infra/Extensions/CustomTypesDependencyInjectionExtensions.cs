using EFCoreApi.Domain;
using EFDataAccess.DataAccess.Accessors;
using Utilities.Authentication;

namespace EFCoreApi.Infra.Extensions;

public static class CustomTypesDependencyInjectionExtensions
{
    public static IServiceCollection AddCustomTypes(this IServiceCollection services)
    {
        services.AddScoped<IRouteMappingAccessor, RouteMappingAccessor>();
        services.AddScoped<IRouteMappingManager, RouteMappingManager>();
        services.AddSingleton<IJwtTokenIssuer, JwtTokenIssuer>();
        return services;
    }
}