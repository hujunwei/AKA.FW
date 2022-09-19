namespace EFCoreApi.DTOs;

public class LoginResponse
{
    public UserDto? UserInfo { get; set; }
    public string? Token { get; set; }
}