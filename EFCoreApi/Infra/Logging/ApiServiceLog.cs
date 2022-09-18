namespace EFCoreApi.Infra.Logging;

public record ApiServiceLog
{
    public string CorrelationId { get; init; } = default!;
    public string Protocol { get; init; } = default!;
    public string AbsoluteUri { get; init; } = default!;
    public string Host { get; init; } = default!;
    public string ApiActionName { get; init; } = default!;
    public string HttpMethod { get; init; } = default!;
    public string RelativeUrl { get; init; } = default!;
    public string Query { get; init; } = default!;
    public long DurationMS { get; init; }
    public int ResultCode { get; init; }
    public string RequesterUpn { get; init; } = default!;
    public string RequestDetails { get; init; } = default!;
    public string ResultMessage { get; init; } = default!;
    public string ApplicationClientIP { get; init; } = default!;
} 