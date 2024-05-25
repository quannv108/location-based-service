using LBS.Dal.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LBS.DbMigrator;

public static class Program
{
    public static void Main(string[] args)
    {
        var configurationBuilder = new ConfigurationBuilder();
        configurationBuilder.AddJsonFile("appsettings.json", false, false);
        configurationBuilder.AddEnvironmentVariables();
        var configuration = configurationBuilder.Build();
        
        var services = new ServiceCollection();
        services.AddDalEf(configuration.GetConnectionString("Default")!);

        var serviceProvider = services.BuildServiceProvider();

        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<LbsDbContext>();
        Console.WriteLine("Start Migration...");
        context.Database.Migrate();
        Console.WriteLine("Migration Done!");
    }
}