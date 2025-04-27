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

        var possibleSamples = transactions.Where(t => t.SplitTransactions.Any());

        var alreadySampledTransactionIds = await db.Context.SpendingSamples
            .Select(spendingSampleGroup => spendingSampleGroup.YnabTransactionDerivedFromId)
            .ToListAsync(stoppingToken);

        var spendingSamples = await db.Context.SpendingSampleMatches
            .Include(ss => ss.Prices)
            .ToListAsync(stoppingToken);

        var unsampledTransactions = possibleSamples.Where(ps
            => ps.SplitTransactions.All(st => st.CanIdentifyIntent()) && !alreadySampledTransactionIds.Contains(ps.Id));

        // TODO: This is asynchronously synchronous, can it be made paralell.
        foreach (var unsampledTransaction in unsampledTransactions)
        {
            PrintToConsole($"Processing {unsampledTransaction.SplitTransactions.Count()} SubTransactions for Transaction: {unsampledTransaction.Id}");
            
            var newlyCreatedSamples = new List<SpendingSampleMatch>();
            foreach (var unsampledSubTransaction in unsampledTransaction.SplitTransactions)
            {
                var foundSample = spendingSamples.FirstOrDefault(u => u.Matches(unsampledSubTransaction));
                
                // No sample found, create the first one with initial pricing.
                if (foundSample == null)
                {
                    PrintToConsole($"Creating New Spending Sample for SubTransaction: {unsampledSubTransaction.Id}");
                    
                    var completelyNewSample = new SpendingSampleMatch
                    {
                        YnabPayeeId = unsampledSubTransaction.PayeeId!.Value,
                        YnabCategoryId = unsampledSubTransaction.CategoryId!.Value,
                        YnabMemo = unsampledSubTransaction.Memo!,
                        Prices = new List<SpendingSampleMatchPrice>
                        {
                            new SpendingSampleMatchPrice
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
                    
                    var newPrice = new SpendingSampleMatchPrice
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

            var group = new SpendingSample
            {
                Created = DateTime.UtcNow, // TODO: Automatically create through EF
                YnabTransactionDerivedFromId = unsampledTransaction.Id,
                YnabPayeeId = unsampledTransaction.PayeeId!.Value,
                Matches = newlyCreatedSamples
            };

            db.Context.SpendingSamples.Add(group);
        }

        await db.Save();
    }
}

public static class SpendingSampleExtensions
{
    public static bool Matches(this SpendingSampleMatch sampleMatch, SplitTransactions splitTransactions)
        => sampleMatch.YnabPayeeId == splitTransactions.PayeeId &&
           sampleMatch.YnabCategoryId == splitTransactions.CategoryId &&
           sampleMatch.YnabMemo == splitTransactions.Memo;
}

public static class SubTransactionExtensions
{
    public static bool CanIdentifyIntent(this SplitTransactions splitTransactions)
        => splitTransactions is { PayeeId: not null, CategoryId: not null, Memo: not null };
}