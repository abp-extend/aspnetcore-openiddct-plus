// See https://aka.ms/new-console-template for more information

using AspNetCoreOpeniddictPlus.Core.Extensions;
using AspNetCoreOpeniddictPlus.Migrator;
using AspNetCoreOpeniddictPlus.Migrator.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serilog;
using Serilog.Events;

public class Program
{
    public static void Main(string[] args)
    {
    
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
            .MinimumLevel.Override("System", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.File("Logs/logs.txt")
            .WriteTo.Console()
            .CreateLogger();
        CreateHostBuilder(args).Build().Run();
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((ctx, configuration) =>
            {
                    configuration.Sources.Clear();
                 
                    configuration.SetBasePath(ctx.HostingEnvironment.ContentRootPath)
                        .AddJsonFile("appsettings.json", false, true)
                        .AddJsonFile($"appsettings.{env}.json", true, true)
                        .AddCommandLine(args)
                        .AddEnvironmentVariables();
            })
            .ConfigureServices((ctx, services) =>
            {
                services.AddLogging(l => l.AddSerilog());
                services.AddOpeniddictPlusDbContext<OpeniddictPlusDbContext>();
                services.AddHostedService<DbMigrator>();
            });

        host.UseEnvironment(env);
        return host;
    }
}