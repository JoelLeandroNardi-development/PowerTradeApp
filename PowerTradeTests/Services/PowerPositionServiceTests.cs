//using Axpo;
//using Moq;
//using PowerTradeCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace PowerTradeTests.Services;

//public class PowerPositionServiceTests
//{
//    [Fact]
//    public async Task GetAggregatedPositionsAsync_ShouldReturnCorrectAggregatedPositions()
//    {
//        // Arrange
//        var mockPowerService = new Mock<IPowerService>();

//        // Mocking a list of trades returned from GetTradesAsync
//        var trades = new List<Trade>
//        {
//            new Trade
//            {
//                Periods = new List<TradePeriod>
//                {
//                    new TradePeriod { Period = 1, Volume = 10 },
//                    new TradePeriod { Period = 2, Volume = 20 },
//                    new TradePeriod { Period = 3, Volume = 30 }
//                }
//            },
//            new Trade
//            {
//                Periods = new List<TradePeriod>
//                {
//                    new TradePeriod { Period = 1, Volume = 5 },
//                    new TradePeriod { Period = 2, Volume = 10 },
//                    new TradePeriod { Period = 3, Volume = 15 }
//                }
//            }
//        };

//        // Mocking the GetTradesAsync method to return the trades list
//        mockPowerService.Setup(service => service.GetTradesAsync(It.IsAny<DateTime>()))
//            .ReturnsAsync(trades);

//        // Prepare the input parameters
//        DateTime startDate = new DateTime(2025, 03, 24); // Example date

//        // Act
//        var result = await PowerPositionService.GetAggregatedPositionsAsync(mockPowerService.Object, startDate);

//        // Assert
//        var accumulatedPositions = result.ToList();

//        // Check the number of positions returned (should be 24 hourly positions)
//        Assert.Equal(24, accumulatedPositions.Count);

//        // Verify that the first position corresponds to the first period (period 1)
//        var firstPosition = accumulatedPositions[0];
//        Assert.Equal(startDate.AddHours(0).ToUniversalTime().ToString(Constants.Iso8601Format), firstPosition.TradeDate);
//        Assert.Equal(15.0, firstPosition.Volume); // 10 (from 1st trade) + 5 (from 2nd trade)

//        // Verify the second position (period 2)
//        var secondPosition = accumulatedPositions[1];
//        Assert.Equal(startDate.AddHours(1).ToUniversalTime().ToString(Constants.Iso8601Format), secondPosition.TradeDate);
//        Assert.Equal(30.0, secondPosition.Volume); // 20 + 10

//        // Verify that volumes are rounded to 2 decimals
//        var roundedVolume = Math.Round(25.12345, 2);
//        Assert.Equal(roundedVolume, accumulatedPositions[0].Volume);
//    }

//    [Fact]
//    public async Task GetAggregatedPositionsAsync_ShouldHandleEmptyTrades()
//    {
//        // Arrange
//        var mockPowerService = new Mock<IPowerService>();

//        // Mocking an empty trades list
//        mockPowerService.Setup(service => service.GetTradesAsync(It.IsAny<DateTime>()))
//            .ReturnsAsync(new List<Trade>());

//        DateTime startDate = new DateTime(2025, 03, 24); // Example date

//        // Act
//        var result = await PowerPositionService.GetAggregatedPositionsAsync(mockPowerService.Object, startDate);

//        // Assert
//        var accumulatedPositions = result.ToList();

//        // Ensure no trades means 0 volume for each hour
//        Assert.Equal(24, accumulatedPositions.Count);
//        foreach (var position in accumulatedPositions)
//        {
//            Assert.Equal(0, position.Volume); // No trades, so volume should be 0
//        }
//    }

//    [Fact]
//    public async Task GetAggregatedPositionsAsync_ShouldFormatDateInIso8601()
//    {
//        // Arrange
//        var mockPowerService = new Mock<IPowerService>();

//        // Mocking a list of trades with sample data
//        var trades = new List<Trade>
//        {
//            new Trade
//            {
//                Periods = new List<TradePeriod>
//                {
//                    new TradePeriod { Period = 1, Volume = 10 },
//                    new TradePeriod { Period = 2, Volume = 20 }
//                }
//            }
//        };

//        // Mocking the GetTradesAsync method to return the trades list
//        mockPowerService.Setup(service => service.GetTradesAsync(It.IsAny<DateTime>()))
//            .ReturnsAsync(trades);

//        DateTime startDate = new DateTime(2025, 03, 24); // Example date

//        // Act
//        var result = await PowerPositionService.GetAggregatedPositionsAsync(mockPowerService.Object, startDate);

//        // Assert
//        var accumulatedPositions = result.ToList();

//        // Verify that the first position has the date formatted in ISO 8601
//        var firstPosition = accumulatedPositions[0];
//        var expectedDate = startDate.AddHours(0).ToUniversalTime().ToString(Constants.Iso8601Format);
//        Assert.Equal(expectedDate, firstPosition.TradeDate);
//    }
//}
