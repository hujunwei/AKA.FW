using EFCoreApi.Infra.Logging;

namespace EFCoreApi.Infra.Middlewares;

public class ApiRequestTrackingMiddleware : BaseMiddleware
{
    public ApiRequestTrackingMiddleware(RequestDelegate next) 
        : base(next)
    { }

    protected override async Task OnExecuting(HttpContext httpContext, Dictionary<string, object> middlewareContext)
    {
        await ApiLogger.StartTracking(httpContext);
    }
}