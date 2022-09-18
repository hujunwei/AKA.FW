namespace EFCoreApi.Infra.Middlewares;

public abstract class BaseMiddleware
{
    private const string STREAM_INSPECTED_FLAG_KEY = "STREAM_INSPECTED";
    // 8192 works for most of logging case
    // It has better performance as it's largest multiple of 4096 which was used in lots of .NET stream/encoder library API
    private const int MAX_LOG_LENGTH_IN_BYTES = 8192;

    private readonly RequestDelegate _next;

    protected BaseMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    // AOP
    protected virtual async Task InvokeImplAsync(HttpContext httpContext, Dictionary<string, object> middlewareContext)
    {
        // pre execution
        await OnExecuting(httpContext, middlewareContext);

        // next middleware
        await NextAsync(httpContext);

        // post execution
        await OnExecuted(httpContext, middlewareContext);
    }

    protected virtual Task OnExecuting(HttpContext httpContext, Dictionary<string, object> middlewareContext)
    {
        return Task.CompletedTask;
    }

    protected virtual Task OnExecuted(HttpContext httpContext, Dictionary<string, object> middlewareContext)
    {
        return Task.CompletedTask;
    }

    private async Task NextAsync(HttpContext httpContext)
    {
        await _next.Invoke(httpContext);
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        var streamInspected = 
            httpContext.Items.ContainsKey(STREAM_INSPECTED_FLAG_KEY) &&
            (bool) httpContext.Items[STREAM_INSPECTED_FLAG_KEY]!;
    
        if (!streamInspected)
        {
            // enable rewind for request stream
            // see https://devblogs.microsoft.com/dotnet/re-reading-asp-net-core-request-bodies-with-enablebuffering/
            if (httpContext.Request.Body != null)
            {
                httpContext.Request.EnableBuffering();
            }
            
            // Wrap the IO response stream with a inspection stream for response logging purpose
            httpContext.Response.Body = new InspectionWriteStream(httpContext.Response.Body, MAX_LOG_LENGTH_IN_BYTES);
    
            // set flag to true
            httpContext.Items[STREAM_INSPECTED_FLAG_KEY] = true;
        }
    
        var middlewareContext = new Dictionary<string, object>();
        await InvokeImplAsync(httpContext, middlewareContext);
    }
}

internal class InspectionWriteStream : Stream
{
    private readonly Stream _innerStream;
    private readonly int _maxLength;
    private readonly MemoryStream _dumpStream;

    public InspectionWriteStream(Stream innerStream, int maxLength)
    {
        _innerStream = innerStream;
        _maxLength = maxLength;

        _dumpStream = new MemoryStream();
    }

    public override bool CanRead => _innerStream.CanRead;

    public override bool CanSeek => _innerStream.CanSeek;

    public override bool CanWrite => _innerStream.CanWrite;

    public override long Length => _innerStream.Length;

    public override long Position
    {
        get => _innerStream.Position;
        set => _innerStream.Position = value;
    }

    public override void Flush()
    {
        _innerStream.Flush();
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        return _innerStream.Read(buffer, offset, count);
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        return _innerStream.Seek(offset, origin);
    }

    public override void SetLength(long value)
    {
        _innerStream.SetLength(value);
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        _innerStream.Write(buffer, offset, count);

        var remainingCapacity = _maxLength - (int)_dumpStream.Length;
        if (remainingCapacity <= 0)
        {
            // Dump stream is full
            return;
        }

        var bytesToDump = Math.Min(count, remainingCapacity);
        _dumpStream.Write(buffer, offset, bytesToDump);
    }

    // In-memory data, no async
    public string GetInspectedText()
    {
        _dumpStream.Position = 0;
        return new StreamReader(_dumpStream).ReadToEnd();
    }
}