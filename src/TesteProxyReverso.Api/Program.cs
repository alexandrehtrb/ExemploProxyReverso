using Serilog;
using TesteProxyReverso.Api.Configurations;
using TesteProxyReverso.Api.Transformers;

namespace TesteProxyReverso.Api;

public static class Program
{
    public static int Main(string[] args)
    {
        var configuration = LoadConfigs();
        SetupSerilog(configuration);

        try
        {
            Log.Information("Starting web host");
            BuildApp(args, configuration).Run();
            return 0;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly");
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    private static IConfiguration LoadConfigs() =>
        new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

    private static void SetupSerilog(IConfiguration configuration) =>
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext()
            .Enrich.WithProperty("ApplicationName", typeof(Program).Assembly.GetName().Name)
            .WriteTo.Console()
            .CreateLogger();

    private static WebApplication BuildApp(string[] args, IConfiguration configuration)
    {
        var webAppBuilder = WebApplication.CreateBuilder(args);

        webAppBuilder.Host.ConfigureHost();
        webAppBuilder.Services.ConfigureServices(configuration);

        var webApp = webAppBuilder.Build();
        webApp.ConfigureApp();

        return webApp;
    }

    private static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddSerilogLogger();

        services
            .AddControllers();

        services
            .AddReverseProxy()
            .LoadFromConfig(configuration.GetSection("ReverseProxy"))
            .AddTransformFactory<MyTransformFactory>();
    }

    private static void ConfigureHost(this ConfigureHostBuilder builder) =>
        builder.UseSerilog();

    private static IApplicationBuilder ConfigureApp(this IApplicationBuilder app) =>
        app
            .UseRouting()
            .UseEndpoints(endpoints =>
            {
                endpoints.MapReverseProxy();
                endpoints.MapControllers();
            });
}