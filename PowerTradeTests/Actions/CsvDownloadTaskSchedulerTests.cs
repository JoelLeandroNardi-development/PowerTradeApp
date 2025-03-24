using Moq;
using PowerTradeCore;
using Microsoft.AspNetCore.Http.HttpResults;

namespace PowerTradeTests;

public class CsvDownloadTaskSchedulerTests
{
    [Fact]
    public async Task ScheduleTask_ShouldCallScheduleMethod_WhenCalled()
    {
        // Arrange
        var mockRecurrentTaskScheduler = new Mock<IRecurrentTaskScheduler>();
        var input = new TaskSchedulerInput(10, "some/folder");

        // Act
        var result = await CsvDownloadTaskScheduler.ScheduleTask(mockRecurrentTaskScheduler.Object, input);

        // Assert
        mockRecurrentTaskScheduler
            .Verify(scheduler => scheduler.Schedule(input.IntervalMinutes, input.FolderPath), Times.Once);
        Assert.IsType<Ok<string>>(result);
    }

    [Fact]
    public async Task ScheduleTask_ShouldReturnOkResult_WhenScheduleSucceeds()
    {
        // Arrange
        var mockRecurrentTaskScheduler = new Mock<IRecurrentTaskScheduler>();
        var input = new TaskSchedulerInput(15, "another/folder");

        // Act
        var result = await CsvDownloadTaskScheduler.ScheduleTask(mockRecurrentTaskScheduler.Object, input);

        // Assert
        Assert.IsType<Ok<string>>(result);
    }

    [Fact]
    public async Task ScheduleTask_ShouldHandleException_WhenScheduleFails()
    {
        // Arrange
        var mockRecurrentTaskScheduler = new Mock<IRecurrentTaskScheduler>();
        var input = new TaskSchedulerInput(10, "some/folder");
        mockRecurrentTaskScheduler
            .Setup(scheduler => scheduler.Schedule(input.IntervalMinutes, input.FolderPath))
            .ThrowsAsync(new Exception("Failed to schedule"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() =>
            CsvDownloadTaskScheduler.ScheduleTask(mockRecurrentTaskScheduler.Object, input));
        Assert.Equal("Failed to schedule", exception.Message);
    }
}
