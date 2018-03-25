using Microsoft.EntityFrameworkCore;

namespace Toffees.Glucose.Data
{
    public class GlucoseDbContext : DbContext
    {
        public GlucoseDbContext(DbContextOptions<GlucoseDbContext> options) : base(options) { }

        public DbSet<Entities.Glucose> Glucoses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=DESKTOP-M7A3T46;Database=GlucoseStore;Trusted_Connection=True;");
            }
        }
    }
}