using Axpo;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace PowerTradeCore;

public static class AppBuilderExtension
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services, bool fromConsole)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File(
                !fromConsole ? Constants.LogsPath : Constants.FromConsolePath + Constants.LogsPath,
                rollingInterval: RollingInterval.Day) 
            .CreateLogger();

        // Add logging to the DI container
        services.AddLogging(builder =>
        {
            builder.AddSerilog();
            builder.SetMinimumLevel(LogLevel.Information);
        });

        services.AddSingleton<IPowerService, PowerService>();
        services.AddSingleton<IPowerPositionService, PowerPositionService>();
        services.AddSingleton<ICsvGenerator, CsvGenerator>();
        services.AddSingleton<IRecurrentTaskScheduler, RecurrentTaskScheduler>();
        services.AddSingleton<ICsvExtractor, CsvExtractor>();
        services.AddHostedService<CsvExtractionWorker>();
        return services;
    }
}
