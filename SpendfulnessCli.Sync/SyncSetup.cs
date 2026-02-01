using Microsoft.Extensions.DependencyInjection;
using Spendfulness.Database;
using Spendfulness.Database.Sqlite;
using SpendfulnessCli.Sync.Synchronisers;
using YnabSharp.Extensions;

namespace SpendfulnessCli.Sync;

public static class SyncSetup
{
    public static void Setup(this IServiceCollection serviceCollection)
    {
        // Dependencies
        serviceCollection
            .AddYnab()
            .AddSpendfulnessSqliteDb();
        
        // Sync-related
        serviceCollection
            .AddHostedService<DatabaseSynchroniser>();  // Ensure db is created with testing settings..
        // .AddHostedService<CommitmentSynchroniser>()
        // .AddHostedService<DerivableSpendingSampleSynchroniser>();
        // .AddHostedService<PossibleSpendingSampleSynchroniser>();
    }
}