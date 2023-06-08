using System.Security.Claims;
using Microsoft.AspNetCore.Http;


namespace Utilities;

/// <summary>
/// Utility for getting http context data 
/// </summary>
public static class ServiceRuntimeContext
{
    private static IHttpContextAccessor _httpContextAccessor = default!;

    private static HttpContext? HttpContext => _httpContextAccessor.HttpContext;

    // Default to 1.0.0 for local test usage.
    public static string ApplicationVersion { get; set; } = "1.0.0";

    public static void Configure(IHttpContextAccessor contextAccessor)
    {
        _httpContextAccessor = contextAccessor;
    }

    public static string ApiActionName
    {
        get => GetContextItemValue("ApiActionName") as string ?? string.Empty;
        set => SetContextItemValue("ApiActionName", value);
    }

    public static string CorrelationId
    {
        get => GetContextItemValue("CorrelationId") as string ?? string.Empty;
        set => SetContextItemValue("CorrelationId", value);
    }

    public static ClaimsPrincipal? CurrentUserClaims => HttpContext?.User;

    public static CancellationToken ServerTimeoutCancellationToken
    {
        get => GetContextItemValue("ServerTimeoutCancellationToken") == null
            ? default
            : (CancellationToken) (GetContextItemValue("ServerTimeoutCancellationToken") ?? CancellationToken.None);
        set => SetContextItemValue("ServerTimeoutCancellationToken", value);
    }

    // helpers below
    private static object? GetContextItemValue(string key)
    {
        if (HttpContext == null)
        {
            return null;
        }

        if (!HttpContext.Items.ContainsKey(key))
        {
            return null;
        }

        return HttpContext.Items[key];
    }

    private static void SetContextItemValue(string key, object value)
    {
        if (HttpContext == null)
        {
            return;
        }

        HttpContext.Items[key] = value;
    }
}