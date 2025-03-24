﻿using PowerTradeCore;

namespace PowerTradeAPI;

public static class Endpoints
{
    public static void AddEndpoints(this WebApplication app)
    {
        app.MapGet("/powerposition/csv", GetPowerPositionCSV.GenerateCSVAsync)
            .WithName("GetCSV")
            .WithOpenApi();
    }
}
