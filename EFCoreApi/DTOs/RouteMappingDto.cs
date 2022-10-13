using FluentValidation;

namespace EFCoreApi.DTOs;

public class RouteMappingDto
{
    public Guid Id { get; set; } = default!;
    public string Name { get; init; } = default!;
    public string SourceAlias { get; init; } = default!;
    public string TargetUrl { get; init; } = default!;

    public bool IsActive { get; init; }
    public bool IsOfficial { get; init; }

    // This is upn instead of uid
    public string? CreatedBy { get; set; } = default!;
    public DateTime? CreatedAt { get; set; } = default!;
    
    // This is upn instead of uid
    public string? UpdatedBy { get; set; } = default!;
    public DateTime? UpdatedAt { get; set; } = default!;

    /// <summary>
    /// This field will be used in optimistic concurrency handling.
    /// DO NOT read/write its value outside data accessor.
    /// </summary>
    public byte[]? _RowVersion { get; set; } = default!;
}

// Format of the fields should be validated in front-end.
public class RouteMappingDtoValidator : AbstractValidator<RouteMappingDto>
{
    // UI reserved URLs.
    private static readonly string[] s_predefinedUrls = { "official", "tools", "my", "authentication", "authentication/sign-in", "authentication/sign-up" };

    public RouteMappingDtoValidator()
    {
        RuleFor(p => p.SourceAlias)
            .Must(sourceAlias => !s_predefinedUrls.Contains(sourceAlias.ToLower()) && !sourceAlias.Contains('/'));
        RuleFor(p => p.TargetUrl).Must(url => !string.IsNullOrWhiteSpace(url));
    }
}