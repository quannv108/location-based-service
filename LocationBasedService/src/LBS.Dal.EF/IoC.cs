using LBS.Dal.EF.Repositories;
using LBS.Dal.Models;
using LBS.Dal.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace LBS.Dal.EF;

public static class IoC
{
    public static IServiceCollection AddDalEf(this IServiceCollection services, string connectionString)
    {
        // https://www.npgsql.org/efcore/mapping/nts.html?tabs=with-datasource
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        dataSourceBuilder.UseNetTopologySuite();
        var dataSource = dataSourceBuilder.Build();

        services.AddDbContext<LbsDbContext>(options =>
        {
            options.UseNpgsql(dataSource, o =>
            {
                o.UseNetTopologySuite();
                var migrationsAssemblyName = typeof(LbsDbContext).Assembly.GetName().Name!;
                o.MigrationsAssembly(migrationsAssemblyName);
            });
        });

        // repositories
        services.AddScoped<IRepository<NamedLocation>, NamedLocationRepository>();
        services.AddScoped<INamedLocationRepository, NamedLocationRepository>();

        return services;
    }
}