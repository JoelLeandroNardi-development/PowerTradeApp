// See https://aka.ms/new-console-template for more information
using PowerTradeCore;
using Axpo;

Console.WriteLine("Hello, World!");
var data = await PowerPositionService.GetAggregatedPositionsCsvAsync(new PowerService(), DateTime.Now);

