namespace PowerTradeCore;

public interface IRecurrentTaskScheduler
{
    Task Schedule(int? intervalMinutes, string? folderPath);
    Task ScheduleFromConsole(int? intervalMinutes, string? folderPath);
}