using Axpo;
using Microsoft.Extensions.DependencyInjection;

namespace PowerTradeCore;

public static class AppBuilderExtension
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddSingleton<IPowerService, PowerService>();
        services.AddSingleton<IPowerPositionService, PowerPositionService>();
        services.AddSingleton<ICsvGenerator, CsvGenerator>();
        services.AddSingleton<IRecurrentTaskScheduler, RecurrentTaskScheduler>();
        services.AddSingleton<ICsvExtractor, CsvExtractor>();
        services.AddHostedService<CsvExtractionWorker>();
        return services;
    }
}
