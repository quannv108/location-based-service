using LBS.Dal.Models;
using Microsoft.EntityFrameworkCore;

namespace LBS.Dal.EF;

public class LbsDbContext : DbContext
{
    public LbsDbContext(DbContextOptions<LbsDbContext> optionsBuilder) : base(optionsBuilder)
    {
    }

    public virtual DbSet<NamedLocation> NamedLocations { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("postgis");
    }
}