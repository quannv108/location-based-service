using LBS.Core;
using LBS.Dal.EF;

namespace LBS.App.HttpApi;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var configuration = builder.Configuration;

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        // App
        builder.Services.Configure<RouteOptions>(o =>
        {
            o.LowercaseUrls = true;
            o.LowercaseQueryStrings = true;
        });
        builder.Services.AddControllers();
        
        // Core
        builder.Services.AddCoreServices();
        
        // Dal
        builder.Services.AddDalEf(configuration.GetConnectionString("Default")!);
        
        // healthcheck
        builder.Services.AddHealthChecks()
            .AddDbContextCheck<LbsDbContext>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        // if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();
        
        // Health Checks endpoint
        app.MapHealthChecks("/health");

        app.MapControllers();

        app.Run();
    }
}