namespace PowerTradeConsole;

public static class MenuScreener
{
    public static void Run()
    {
        Console.WriteLine("Welcome to the Power Position CSV Downloader.");
        Console.WriteLine("============================================================");
        Console.WriteLine("Select an option:");
        Console.WriteLine("1 - Download CSV to the default folder (set in configuration file)");
        Console.WriteLine("2 - Download CSV to a custom folder (this option also changes configuration file");
        Console.WriteLine("3 - Change default folder path");
        Console.WriteLine("4 - Change interval of periodical download of CSV");
        Console.WriteLine("5 - Exit");
        Console.WriteLine("============================================================");
    }
}
