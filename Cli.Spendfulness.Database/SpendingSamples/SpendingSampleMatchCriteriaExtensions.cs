using Ynab;

namespace Cli.Spendfulness.Database.SpendingSamples;

public static class SpendingSampleMatchCriteriaExtensions
{
    public static bool SimilarTo(this SpendingSampleMatchCriteria sampleMatch, SplitTransactions splitTransactions)
        => sampleMatch.YnabPayeeId == splitTransactions.PayeeId &&
           sampleMatch.YnabCategoryId == splitTransactions.CategoryId &&
           sampleMatch.YnabMemo == splitTransactions.Memo;
    
    public static void EnsurePriceExists(this SpendingSampleMatchCriteria sampleMatch, decimal amount)
    {
        var matchedCriteriaPrice = sampleMatch.Prices.FirstOrDefault(price => price.Amount == amount);
        if (matchedCriteriaPrice != null)
        {
            return;
        }

        var criteriaPrice = new SpendingSampleMatchCriteriaPrice
        {
            EffectiveFrom = DateTime.UtcNow,
            Amount =amount,
        };

        sampleMatch.Prices.Add(criteriaPrice);
    }
}