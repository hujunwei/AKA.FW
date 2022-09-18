using EFCoreApi.Infra.Logging;

namespace EFCoreApi.Infra.Extensions;

public static class ApiLoggerExtension
{
    public static IApplicationBuilder UseStaticApiLogger(this WebApplication app)
    {
        ApiLogger.Configure(app.Logger);

        return app;
    }
}