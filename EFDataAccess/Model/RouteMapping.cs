using EFDataAccess.Model.Common;

namespace EFDataAccess.Model;

public class RouteMapping : ChangeableEntity<Guid>
{
    public string Name { get; set; } = default!;

    public string ShortUrl { get; set; } = default!;
    
    public string TargetUrl { get; set; } = default!;

    public bool IsActive { get; set; } = default!;

    public bool IsOfficial { get; set; } = default!;

    public string ClientData { get; set; } = default!;
}