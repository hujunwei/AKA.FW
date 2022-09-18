using System.Diagnostics;
using Microsoft.AspNetCore.Http.Extensions;
using Newtonsoft.Json;
using Utilities;

namespace EFCoreApi.Infra.Logging;

public static class ApiLogger
{
    private const string REQUEST_BODY_KEY = "REQUEST_BODY";
    private const string STOPWATCH_KEY = "STOPWATCH";
    private const string API_LOGGED_KEY = "API_LOGGED";
    private static ILogger? s_logger;
    private static readonly JsonSerializerSettings _serializerSettings = new()
    {
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        DateTimeZoneHandling = DateTimeZoneHandling.Utc
    };

    public static void Configure(ILogger logger)
    {
        s_logger = logger;
    }

    public static async Task StartTracking(HttpContext httpContext)
    {
        // Dump request body.
        if (httpContext.Request.Body != null)
        {
            httpContext.Items[REQUEST_BODY_KEY] = await new StreamReader(httpContext.Request.Body).ReadToEndAsync();
            httpContext.Request.Body.Seek(0, SeekOrigin.Begin);
        }

        // Start a stopwatch
        httpContext.Items[STOPWATCH_KEY] = Stopwatch.StartNew();
    }

    public static void WriteApiLog(HttpContext httpContext, string responseBody)
    {
        if (httpContext.Items.ContainsKey(API_LOGGED_KEY))
        {
            // Already logged, skip
            return;
        }

        // Record execution duration
        var stopwatch = httpContext.Items[STOPWATCH_KEY] as Stopwatch;
        // Gets duration. Stop watch is started in an Middleware.
        var duration = stopwatch?.ElapsedMilliseconds ?? -1;

        // Write API call log.
        var log = new ApiServiceLog
        {
            CorrelationId = ServiceRuntimeContext.CorrelationId,
            Protocol = httpContext.Request.Scheme,
            AbsoluteUri = httpContext.Request.GetDisplayUrl(),
            Host = httpContext.Request.Host.ToString(),
            ApiActionName = ServiceRuntimeContext.ApiActionName,
            HttpMethod = httpContext.Request.Method,
            RelativeUrl = httpContext.Request.GetEncodedPathAndQuery(),
            Query = httpContext.Request.QueryString.ToString(),
            DurationMS = duration,
            ResultCode = httpContext.Response.StatusCode,
            RequesterUpn = ServiceRuntimeContext.CurrentUserClaims?.Identity?.Name ?? string.Empty, // Warn: PII data
            RequestDetails = httpContext.Items[REQUEST_BODY_KEY] as string ?? string.Empty,
            ApplicationClientIP = httpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty,
            ResultMessage = responseBody
        };

        // REAL API LOGGING HERE.
        s_logger?.Log(httpContext.Response.StatusCode < 400 ? LogLevel.Information : LogLevel.Error, JsonConvert.SerializeObject(log, _serializerSettings));

        httpContext.Items[API_LOGGED_KEY] = true;
    }
}
