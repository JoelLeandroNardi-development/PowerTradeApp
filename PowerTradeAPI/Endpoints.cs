using Microsoft.AspNetCore.Mvc;
using PowerTradeCore;

namespace PowerTradeAPI;

public static class Endpoints
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    public static void AddEndpoints(this WebApplication app)
    {
        app.MapGet("/weatherforecast", () =>
        {
            var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    Summaries[Random.Shared.Next(Summaries.Length)]
                ))
                .ToArray();
            return forecast;
        })
        .WithName("GetWeatherForecast")
        .WithOpenApi();

        app.MapGet("/powerposition/csv", () => GetPowerPositionCSV.GenerateCSVAsync("ss"))
        .WithName("GetCSV")
        .WithGroupName("PowerPosition")
        .WithOpenApi();
    }
}
