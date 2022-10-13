using FluentValidation;

namespace EFCoreApi.DTOs;

public class UserDto
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string NickName { get; set; } = default!;
    public string Email { get; set; } = default!; 
    public bool EmailConfirmed { get; set; }
    public string? PhoneNumber { get; set; } = default!;
    public bool PhoneNumberConfirmed { get; set; }
    public bool TwoFactorEnabled { get; set; }
    public DateTimeOffset LockoutEnd { get; set; } = default!;
    public bool lockoutEnabled { get; set; }
    public DateTime CreationTime { get; set; }
}

public class UserDtoValidator : AbstractValidator<UserDto>
{
    private static readonly string[] whiteListedUser = { "akafwadmin@outlook.com", "jasonhu0614@outlook.com"};

    public UserDtoValidator()
    {
        RuleFor(u => u.UserName).Must(username => username.Contains("freewheel", StringComparison.OrdinalIgnoreCase) || whiteListedUser.Contains(username));
        RuleFor(u => u.Email).Must(email =>
            email.Contains("freewheel", StringComparison.OrdinalIgnoreCase) ||
            whiteListedUser.Contains(email));
        RuleFor(u => u).Must(u => u.UserName == u.Email);
    }
}