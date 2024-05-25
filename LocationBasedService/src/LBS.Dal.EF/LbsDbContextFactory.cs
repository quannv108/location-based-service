using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Npgsql;

namespace LBS.Dal.EF;

public class LbsDbContextFactory : IDesignTimeDbContextFactory<LbsDbContext>
{
    public LbsDbContext CreateDbContext(string[] args)
    {
        // This is a design-time factory, so we don't have access to configuration.
        // We need to create the connection string manually.
        const string connectionString = "Host=localhost;Database=lbs;Username=postgres;Password=postgres";
        
        // https://www.npgsql.org/efcore/mapping/nts.html?tabs=with-datasource
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        dataSourceBuilder.UseNetTopologySuite();
        var dataSource = dataSourceBuilder.Build();

        var optionsBuilder = new DbContextOptionsBuilder<LbsDbContext>();
        optionsBuilder.UseNpgsql(dataSource, builder => builder.UseNetTopologySuite());
        var options = optionsBuilder.Options;

        return new LbsDbContext(options);
    }
}