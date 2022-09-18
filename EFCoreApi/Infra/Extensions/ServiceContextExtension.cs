using Utilities;

namespace EFCoreApi.Infra.Extensions;

public static class ServiceContextExtension
{
    public static IApplicationBuilder UseStaticHttpContext(this WebApplication app)
    {
        var httpContextAccessor = app.Services.GetService<IHttpContextAccessor>();

        ServiceRuntimeContext.Configure(httpContextAccessor!);

        return app;
    }
}