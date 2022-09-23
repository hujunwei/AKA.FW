namespace Utilities.Authentication;

public class JwtOptions
{
    public string SigningKey { get; set; } = default!;
    public int ExpireSeconds { get; set; }
}