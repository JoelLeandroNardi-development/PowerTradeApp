using Axpo;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PowerTradeCore;

public static class AppBuilderExtension
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IPowerService, PowerService>();
        services.AddSingleton<IPowerPositionService, PowerPositionService>();
        services.AddSingleton<ICsvGenerator, CsvGenerator>();
        services.AddSingleton<IRecurrentTaskScheduler, RecurrentTaskScheduler>();
        services.AddHostedService<CsvExtractionWorker>();
        return services;
    }
}
