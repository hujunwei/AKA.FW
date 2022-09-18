using EFCoreApi.Infra.Logging;

namespace EFCoreApi.Infra.Middlewares;

public class ApiLoggerMiddleware : BaseMiddleware
{
    public ApiLoggerMiddleware(RequestDelegate next) : base(next)
    {
    }
    
    protected override Task OnExecuted(HttpContext httpContext, Dictionary<string, object> middlewareContext)
    {
        // Dump response body.
        var responseBody = string.Empty;
        if (httpContext.Response.Body is InspectionWriteStream inspectionStream)
        {
            responseBody = inspectionStream.GetInspectedText();
        }

        ApiLogger.WriteApiLog(httpContext, responseBody);
        
        return Task.CompletedTask;
    }
}