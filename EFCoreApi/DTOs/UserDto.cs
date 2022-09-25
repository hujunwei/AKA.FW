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