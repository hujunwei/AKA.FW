using EFCoreApi.Domain;
using EFDataAccess.DataAccess.Accessors;
using Utilities.Authentication;

namespace EFCoreApi.Infra.Extensions;

public static class CustomTypesDependencyInjectionExtensions
{
    public static IServiceCollection AddCustomTypes(this IServiceCollection services)
    {
        services.AddScoped<IPersonAccessor, PersonAccessor>();
        services.AddScoped<IPeopleManager, PeopleManager>();
        services.AddSingleton<IJwtTokenIssuer, JwtTokenIssuer>();
        return services;
    }
}