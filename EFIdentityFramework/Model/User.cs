using Microsoft.AspNetCore.Identity;

namespace EFIdentityFramework.Model;

public class User : IdentityUser<Guid>
{
    public DateTime CreationTime { get; set; }
    public string? NickName { get; set; }
}