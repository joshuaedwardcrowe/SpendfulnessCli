using Microsoft.Extensions.DependencyInjection;
using Spendfulness.Database;
using Ynab.Extensions;

namespace SpendfulnessCli.Sync;

public static class SyncSetup
{
    public static void Setup(this IServiceCollection serviceCollection)
    {
        // Dependencies
        serviceCollection
            .AddYnab()
            .AddSpendfulnessDb();
        
        // Sync-related
        // serviceCollection
        // .AddHostedService<CommitmentSynchroniser>()
        // .AddHostedService<DerivableSpendingSampleSynchroniser>();
        // .AddHostedService<PossibleSpendingSampleSynchroniser>();
    }
}