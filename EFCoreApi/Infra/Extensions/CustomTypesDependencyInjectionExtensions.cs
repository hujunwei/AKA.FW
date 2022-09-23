using Utilities.Authentication;

namespace EFCoreApi.Infra.Extensions;

public static class CustomTypesDependencyInjectionExtensions
{
    public static IServiceCollection AddCustomTypes(this IServiceCollection services)
    {
        services.AddSingleton<IJwtTokenIssuer, JwtTokenIssuer>();
        return services;
    }
}