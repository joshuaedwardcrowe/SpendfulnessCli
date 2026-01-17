using Microsoft.Extensions.DependencyInjection;
using Spendfulness.Csv;
using Spendfulness.Database;
using SpendfulnessCli.Sync.Exports;
using SpendfulnessCli.Sync.Synchronisers;
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
        serviceCollection
            .AddHostedService<PersonalInflationRateExport>(); 
        // .AddHostedService<CommitmentSynchroniser>()
        // .AddHostedService<DerivableSpendingSampleSynchroniser>();
        // .AddHostedService<PossibleSpendingSampleSynchroniser>();
    }
}