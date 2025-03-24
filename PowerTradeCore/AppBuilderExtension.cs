using Axpo;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PowerTradeCore;

public static class AppBuilderExtension
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IPowerService, PowerService>();
        services.AddHostedService<CsvExtractionWorker>();
        return services;
    }
}
