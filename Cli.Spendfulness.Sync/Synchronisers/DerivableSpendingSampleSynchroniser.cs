using Cli.Spendfulness.Database;
using Cli.Spendfulness.Database.SpendingSamples;
using Ynab;

namespace YnabCli.Sync.Synchronisers;

/// <summary>
/// Derive any possible Spending Samples from Transactions that have already been split.
/// </summary>
public class DerivableSpendingSampleSynchroniser(ConfiguredBudgetClient configuredBudgetClient, YnabCliDb db) : Synchroniser
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        PrintToConsole("Syncing Spending Samples ...");

        var unsampledTransactions = await GetUnsampledTransactions();
        if (unsampledTransactions.Count == 0)
        {
            PrintToConsole("No unsampled transactions found, exiting ...");
            return;
        }

        await SyncDerivableSpendingSamples(unsampledTransactions);
    }

    private async Task<List<Transaction>> GetUnsampledTransactions()
    {
        var budget = await configuredBudgetClient.GetDefaultBudget();
        PrintToConsole($"Syncing Spending Samples for Budget: {budget.Id}");

        // TODO: Add support for 'SpendingSampleImport' timing and filter transactions on it.
        var transactions = await budget.GetTransactions();

        var sampledTransactionIds = await db.GetDerivedSpendingSampleTransactionIds();
        
        return transactions
            .Where(t => t.SplitTransactions.Any() && 
                        t.SplitTransactions.AllFullyFormed() && 
                        !sampledTransactionIds.Contains(t.Id))
            .ToList();
    }
    
    private async Task SyncDerivableSpendingSamples(List<Transaction> unsampledTransactions) 
    {
        var matchCriteria = await db.GetSpendingSampleMatchCriteria();
        
        foreach (var unsampledTransaction in unsampledTransactions)
        {
            PrintToConsole($"Sampling transaction: {unsampledTransaction.Id}");

            var foundMatchCriteria = new List<SpendingSampleMatchCriteria>();
            foreach (var unsampledSplitTransaction in unsampledTransaction.SplitTransactions)
            {
                // Looked through match criteria and add price if missing.
                var existingMatchCriteria = matchCriteria.FirstOrDefault(mc => mc.SimilarTo(unsampledSplitTransaction));
                if (existingMatchCriteria != null)
                {
                    PrintToConsole($"Sampling transaction: {unsampledTransaction.Id} - matched crtieria");
                    
                    // TODO: Modifying an object as you're going along feels like blasphemy.
                    existingMatchCriteria.EnsurePriceExists(unsampledSplitTransaction.Amount);
                    foundMatchCriteria.Add(existingMatchCriteria);
                }

                PrintToConsole($"Sampling transaction: {unsampledTransaction.Id} - no matched criteria");
                var newlyCreatedMatchCrtieria = SpendingSampleMatchCriteria.CreateFrom(unsampledSplitTransaction);
                foundMatchCriteria.Add(newlyCreatedMatchCrtieria);
            }

            var sample = new SpendingSample
            {
                Created = DateTime.UtcNow,
                YnabTransactionId = unsampledTransaction.Id,
                Matches = foundMatchCriteria
            };
            
            db.Context.SpendingSamples.Add(sample);
        }

        await db.Save();
        PrintToConsole("Syncing Spending Samples complete");
    }
}