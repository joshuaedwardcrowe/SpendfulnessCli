using Cli.Spendfulness.Database;
using Microsoft.Extensions.Hosting;

namespace YnabCli.Sync.Synchronisers;

public class DatabaseSynchroniser(YnabCliDbContext dbContext) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await dbContext.Database.EnsureCreatedAsync(stoppingToken);
    }
}