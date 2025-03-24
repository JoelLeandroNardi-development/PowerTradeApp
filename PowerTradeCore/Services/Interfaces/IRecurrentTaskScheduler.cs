namespace PowerTradeCore;

public interface IRecurrentTaskScheduler
{
    Task Schedule(int? intervalMinutes, string? folderPath);
}