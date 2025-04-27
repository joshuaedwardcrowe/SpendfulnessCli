using Ynab;
using YnabCli.Database.SpendingSamples;

namespace YnabCli.Database;

public static class SpendingSampleExtensions
{
    public static bool Matches(this SpendingSample sample, Transaction transaction)
    {
        var mostRecentSampleTotal = sample.Matches.Sum(x => x.MostRecentPrice.Amount);
        
        var allCategoryIds = sample.Matches
            .Select(x => x.YnabCategoryId)
            .Distinct();
        
        // Payee is the same, though not sure this matters.
        return sample.YnabPayeeId == transaction.PayeeId && 
               
               // Transaction amount equals or can be inclusive
               mostRecentSampleTotal <= transaction.Amount && 
               
               // Transaction is categorised like one of the matches
               transaction.CategoryId.HasValue &&
               allCategoryIds.Contains(transaction.CategoryId.Value);
    }
}