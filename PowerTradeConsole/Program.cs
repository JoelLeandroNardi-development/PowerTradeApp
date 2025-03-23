// See https://aka.ms/new-console-template for more information
using PowerTradeCore;

Console.WriteLine("Hello, World!");
var data = await PowerPositionService.GetAggregatedPositionsAsync(new PowerService(), DateTime.Now);

