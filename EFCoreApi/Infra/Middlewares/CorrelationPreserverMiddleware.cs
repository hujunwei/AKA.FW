using Utilities;

namespace EFCoreApi.Infra.Middlewares;

public class CorrelationPreserverMiddleware : BaseMiddleware
{
    public CorrelationPreserverMiddleware(RequestDelegate next) : base(next)
    {
    }
    
    protected override Task OnExecuting(HttpContext httpContext, Dictionary<string, object> middlewareContext)
    {
        // get correlation from request header
        var correlationExists = httpContext.Request.Headers.TryGetValue("CorrelationId", out var headers);

        // set to context
        ServiceRuntimeContext.CorrelationId = correlationExists ? headers.First() : Guid.NewGuid().ToString();

        httpContext.Response.OnStarting( state =>
        {
            if (state is HttpContext context)
            {
                context.Response.Headers.Add("CorrelationId", ServiceRuntimeContext.CorrelationId);
            }
            
            return Task.CompletedTask;
        }, httpContext);

        return base.OnExecuting(httpContext, middlewareContext);
    }
}