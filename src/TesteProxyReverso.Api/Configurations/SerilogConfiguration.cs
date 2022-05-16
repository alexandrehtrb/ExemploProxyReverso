using Serilog;

namespace TesteProxyReverso.Api.Configurations;

internal static class SerilogConfiguration
{
    public static IServiceCollection AddSerilogLogger(this IServiceCollection services)
    {
        if (services == null)
            throw new ArgumentNullException(nameof(services));

        services.AddSingleton(Log.Logger);
        return services;
    }
}