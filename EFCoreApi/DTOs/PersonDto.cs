using FluentValidation;

namespace EFCoreApi.DTOs;

public class PersonDto
{
    public override string ToString()
    {
        return $"{nameof(Id)}: {Id}, {nameof(FirstName)}: {FirstName}, {nameof(LastName)}: {LastName}, {nameof(Age)}: {Age}, {nameof(Addresses)}: {string.Join(",", Addresses)}, {nameof(EmailAddresses)}: {string.Join(",", EmailAddresses)}";
    }

    public int Id { get; set; }
    public string FirstName { get; init; } = default!;
    public string LastName { get; init; } = default!;
    public int? Age { get; init; }

    public List<string> Addresses { get; init; } = new();
    public List<string> EmailAddresses { get; init; } = new();
    
    public string? CreatedBy { get; set; } = default!;

    public DateTime CreatedAt { get; set; } = default!;

    public string? UpdatedBy { get; set; } = default!;

    public DateTime UpdatedAt { get; set; } = default!;

    /// <summary>
    /// This field will be used in optimistic concurrency handling.
    /// DO NOT read/write its value outside data accessor.
    /// </summary>
    public byte[]? _RowVersion { get; set; } = default!;
}

public class PersonDtoValidator : AbstractValidator<PersonDto>
{
    public PersonDtoValidator()
    {
        RuleFor(p => p.FirstName).Must(n => n.Length > 2);
        RuleFor(p => p.LastName).Must(n => n.Length > 2);
        RuleFor(p => p.Age).Must(a => a is > 0 and < 200);
    }
}