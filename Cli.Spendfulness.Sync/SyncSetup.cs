using Cli.Spendfulness.Database;
using Microsoft.Extensions.DependencyInjection;
using Ynab.Extensions;
using YnabCli.Sync.Synchronisers;

namespace YnabCli.Sync;

public static class SyncSetup
{
    public static void Setup(this IServiceCollection serviceCollection)
    {
        // Dependencies
        serviceCollection
            .AddYnab()
            .AddDb();
        
        // Sync-related
        serviceCollection
            .AddHostedService<DatabaseSynchroniser>() // Ensure db is created.
            // .AddHostedService<CommitmentSynchroniser>()
            // .AddHostedService<DerivableSpendingSampleSynchroniser>();
            .AddHostedService<PossibleSpendingSampleSynchroniser>();
    }
}