using Microsoft.EntityFrameworkCore;
using Ynab;
using YnabCli.Database;
using YnabCli.Database.SpendingSamples;

namespace YnabCli.Sync.Synchronisers;

public class SpendingSampleSynchroniser(ConfiguredBudgetClient configuredBudgetClient, YnabCliDb db) : Synchroniser
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var budget = await configuredBudgetClient.GetDefaultBudget();
        PrintToConsole($"Syncing Spending Samples for Budget: {budget.Id}");

        var transactions = await budget.GetTransactions();

        var possibleSamples = transactions.Where(t => t.SubTransactions.Any());

        var alreadySampledTransactionIds = await db.Context.SpendingSampleGroups
            .Select(spendingSampleGroup => spendingSampleGroup.YnabTransactionDerivedFromId)
            .ToListAsync(stoppingToken);

        var spendingSamples = await db.Context.SpendingSamples
            .Include(ss => ss.Prices)
            .ToListAsync(stoppingToken);

        var unsampledTransactions = possibleSamples.Where(ps
            => ps.SubTransactions.All(st => st.CanIdentifyIntent()) && !alreadySampledTransactionIds.Contains(ps.Id));

        // TODO: This is asynchronously synchronous, can it be made paralell.
        foreach (var unsampledTransaction in unsampledTransactions)
        {
            PrintToConsole($"Processing {unsampledTransaction.SubTransactions.Count()} SubTransactions for Transaction: {unsampledTransaction.Id}");
            
            var newlyCreatedSamples = new List<SpendingSample>();
            foreach (var unsampledSubTransaction in unsampledTransaction.SubTransactions)
            {
                var foundSample = spendingSamples.FirstOrDefault(u => u.Matches(unsampledSubTransaction));
                
                // No sample found, create the first one with initial pricing.
                if (foundSample == null)
                {
                    PrintToConsole($"Creating New Spending Sample for SubTransaction: {unsampledSubTransaction.Id}");
                    
                    var completelyNewSample = new SpendingSample
                    {
                        YnabPayeeId = unsampledSubTransaction.PayeeId.Value,
                        YnabCategoryId = unsampledSubTransaction.CategoryId.Value,
                        YnabMemo = unsampledSubTransaction.Memo,
                        Prices = new List<SpendingSamplePrices>
                        {
                            new SpendingSamplePrices
                            {
                                EffectiveFrom = unsampledTransaction.Occured,
                                Amount = unsampledSubTransaction.Amount,
                            }
                        }
                    };
                    
                    // Add a new sample to be saved into the colleciton.
                    newlyCreatedSamples.Add(completelyNewSample);
                    continue;
                }
                
                PrintToConsole($"Found Existing Spending Sample for Transaction: {unsampledTransaction.Id} as Sample: {foundSample.Id}");
                
                var foundPricing = foundSample.Prices.FirstOrDefault(p => p.Amount == unsampledSubTransaction.Amount);
                
                // I found a sample, but no pricings match the one i need.
                if (foundPricing == null)
                {
                    PrintToConsole($"Creating New Price for SubTransaction: {unsampledSubTransaction.Id} in Sample: {foundSample.Id}");
                    
                    var newPrice = new SpendingSamplePrices
                    {
                        Amount = unsampledSubTransaction.Amount, // TODO: move to constructor.
                    };
                    
                    // Just add the pricing to be saved later.
                    foundSample.Prices.Add(newPrice);
                    
                    newlyCreatedSamples.Add(foundSample);
                    continue;
                }
                
                PrintToConsole($"Found Existing Price for SubTransaction: {unsampledSubTransaction.Id} as Price {foundPricing.Id}");
            }

            var group = new SpendingSampleGroup
            {
                Created = DateTime.UtcNow, // TODO: Automatically create through EF
                YnabTransactionDerivedFromId = unsampledTransaction.Id,
                Samples = newlyCreatedSamples
            };

            db.Context.SpendingSampleGroups.Add(group);
        }

        await db.Save();
    }
}

public static class SpendingSampleExtensions
{
    public static bool Matches(this SpendingSample sample, SubTransaction transaction)
        => sample.YnabPayeeId == transaction.PayeeId &&
           sample.YnabCategoryId == transaction.CategoryId &&
           sample.YnabMemo == transaction.Memo;
}

public static class SubTransactionExtensions
{
    public static bool CanIdentifyIntent(this SubTransaction subTransaction)
        => subTransaction is { PayeeId: not null, CategoryId: not null, Memo: not null };
}