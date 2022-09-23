using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFDataAccess.Model;

public class RouteMappingConfig : IEntityTypeConfiguration<RouteMapping>
{
    public void Configure(EntityTypeBuilder<RouteMapping> builder)
    {
        builder.ToTable("RouteMapping");

        builder.Property(p => p.Id).HasMaxLength(50);
        builder.Property(p => p.Name).HasMaxLength(200);
        builder.Property(p => p.ShortUrl).HasMaxLength(100);
        builder.Property(p => p.TargetUrl).HasMaxLength(500);
        builder.Property(p => p.CreatedBy).HasMaxLength(50);
        builder.Property(p => p.UpdatedBy).HasMaxLength(50);
        builder.Property(p => p._RowVersion).IsRowVersion();
    }
}