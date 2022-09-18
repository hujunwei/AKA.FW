using System.Net;
using System.Security.Authentication;
using EFCoreApi.Infra.Logging;
using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;
using Utilities;

namespace EFCoreApi.Infra.Handlers;

/// <summary>
/// Map generic exception to HttpStatusCode
/// Expand aggregated exception
/// Write to HttpResponse
/// </summary>
public static class GeneralExceptionHandler
{
    private static readonly Dictionary<Type, HttpStatusCode> _exceptionTypeHttpStatusCodeMapping = new()
    {
        { typeof(ArgumentException), HttpStatusCode.BadRequest },
        { typeof(ArgumentNullException), HttpStatusCode.BadRequest },
        { typeof(NotSupportedException), HttpStatusCode.NotImplemented },
        { typeof(NotImplementedException), HttpStatusCode.NotImplemented },
        { typeof(AuthenticationException), HttpStatusCode.Unauthorized },
        { typeof(InvalidCredentialException), HttpStatusCode.Unauthorized },
        //{ typeof(UnauthorizedException), HttpStatusCode.Forbidden },
        { typeof(UnauthorizedAccessException), HttpStatusCode.Forbidden },
        { typeof(InvalidOperationException), HttpStatusCode.MethodNotAllowed },
        { typeof(OperationCanceledException), HttpStatusCode.BadRequest },
        { typeof(KeyNotFoundException), HttpStatusCode.BadRequest },
        { typeof(TimeoutException), HttpStatusCode.RequestTimeout },
        //{ typeof(EntityNotFoundException<>), HttpStatusCode.NotFound },
        { typeof(EntityChangedException), HttpStatusCode.PreconditionFailed },
        { typeof(EntityAlreadyExistsException), HttpStatusCode.Conflict },
        { typeof(EntityUpdateException), HttpStatusCode.InternalServerError }, // dont really know what happened so make it 500 for now
    };

    private static readonly JsonSerializerSettings _serializerSettings = new()
    {
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        DateTimeZoneHandling = DateTimeZoneHandling.Utc
    };

    public static async Task HandleException(HttpContext httpContext)
    {
        var exceptionHandlerFeature = httpContext.Features.Get<IExceptionHandlerFeature>();
        var exception = exceptionHandlerFeature!.Error;

        // try to extract the inner exception if it is an AggregateException
        var actualException = exception;
        if (actualException.GetType() == typeof(AggregateException) && actualException.InnerException != null)
        {
            actualException = actualException.InnerException;
        }

        // get status code
        var httpStatusCode = HttpStatusCode.InternalServerError;

        var actualExceptionType = actualException.GetType().IsGenericType
            ? actualException.GetType().GetGenericTypeDefinition()
            : actualException.GetType();

        if (httpContext.RequestAborted.IsCancellationRequested)
        {
            // Different libraries will throw different types of exception when task is cancelled
            // If the TaskCanceledException is triggered by RequestAborted, set the status code to BadRequest
            httpStatusCode = HttpStatusCode.BadRequest;
        }
        // else if (actualException is RecordException re)
        // {
        //     // Use the HttpStatusCode of CosmosDB library. Default to InternalServerError
        //     httpStatusCode = re.HttpStatusCode == 0
        //         ? HttpStatusCode.InternalServerError
        //         : (HttpStatusCode)re.HttpStatusCode;
        // }
        // else if (actualException is CosmosException ce)
        // {
        //     // Use the HttpStatusCode of CosmosDB library.
        //     httpStatusCode = ce.StatusCode;
        // }
        // else if (actualException is ExternalServiceException ese)
        // {
        //     httpStatusCode = MapExternalServiceExceptionToStatusCode(ese);
        // }
        else if (_exceptionTypeHttpStatusCodeMapping.ContainsKey(actualExceptionType))
        {
            httpStatusCode = _exceptionTypeHttpStatusCodeMapping[actualExceptionType];
        }

        // set response status code
        httpContext.Response.StatusCode = (int)httpStatusCode;

        // WriteApiLog with full stack trace
        var output = new Dictionary<string, object>
        {
            ["Message"] = actualException.Message,
            ["StackTrace"] = actualException.StackTrace ?? string.Empty,
            ["ExceptionMessage"] = actualException.Message,
            ["ExceptionType"] = actualException.GetType().Name,
            ["InnerException"] = actualException.InnerException ?? new Exception(""),
            ["CorrelationId"] = ServiceRuntimeContext.CorrelationId,
            ["TimeStamp"] = DateTime.UtcNow.ToString("o"),
        };

        // if (actualException is IAdditionalExceptionInfo aei)
        // {
        //     output["AdditionalInfo"] = aei.GetAdditionalExceptionInfo();
        // }

        // Log full information including StackTrace and InnerException
        ApiLogger.WriteApiLog(httpContext, JsonConvert.SerializeObject(output, _serializerSettings));

        // Security: Remove StackTrace & InnerException from the HTTP response
        // output.Remove("StackTrace");
        // output.Remove("InnerException");
        
        await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(output, _serializerSettings));
    }

    // private static HttpStatusCode MapExternalServiceExceptionToStatusCode(ExternalServiceException ese)
    // {
    //     if (ese.StatusCode == HttpStatusCode.RequestTimeout)
    //     {
    //         // Request timeout status code that's being used by external services to indicate service timed out should be
    //         // converted to 500 errors.
    //         return HttpStatusCode.InternalServerError;
    //     }
    //     else
    //     {
    //         return ese.StatusCode;
    //     }
    // }
}