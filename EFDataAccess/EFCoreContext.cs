using EFDataAccess.Model;
using Microsoft.EntityFrameworkCore;

namespace EFDataAccess {
    public class EFCoreContext : DbContext {
        public EFCoreContext() { }

        public EFCoreContext(DbContextOptions<EFCoreContext> options) : base(options) {}

        public DbSet<RouteMapping> RouteMappings { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // This will register or config extensions in current class's assembly
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}

