using EFDataAccess.Model;
using Microsoft.EntityFrameworkCore;

namespace EFDataAccess {
    public class EFCoreDemoContext : DbContext {
        public EFCoreDemoContext() { }

        public EFCoreDemoContext(DbContextOptions<EFCoreDemoContext> options) : base(options) {}

        public DbSet<Person> People { get; set; } = default!;
        public DbSet<Address> Addresses { get; set; } = default!;
        public DbSet<Email> EmailAddresses { get; set; } = default!;

        public DbSet<RouteMapping> RouteMappings { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // This will register or config extensions in current class's assembly
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}

