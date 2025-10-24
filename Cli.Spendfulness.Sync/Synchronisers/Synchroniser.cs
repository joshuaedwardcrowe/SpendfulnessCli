using Microsoft.Extensions.Hosting;

namespace YnabCli.Sync.Synchronisers;

public abstract class Synchroniser : BackgroundService
{
    protected void PrintToConsole(string message)
        => Console.WriteLine($"[{nameof(Synchroniser)}] - {message}");
}