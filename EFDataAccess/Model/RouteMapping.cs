using EFDataAccess.Model.Common;

namespace EFDataAccess.Model;

public class RouteMapping : ChangeableEntity<Guid>
{
    public string Name { get; set; } = default!;

    public string SourceAlias { get; set; } = default!;
    
    public string TargetUrl { get; set; } = default!;

    public bool IsActive { get; set; }

    public bool IsOfficial { get; set; }

    public string? ClientData { get; set; } = default!;
}