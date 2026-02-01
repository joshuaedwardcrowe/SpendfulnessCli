using YnabSharp;

namespace Spendfulness.Aggregation.Aggregator.AmountAggregators;

public class CategoryDeductedAmountYnabListAggregator(IEnumerable<Account> accounts, IEnumerable<CategoryGroup> categoryGroups)
    : YnabListAggregator<decimal>(accounts, categoryGroups)
{
    protected override IEnumerable<decimal> GenerateAggregate()
    {
        var availableAccountBalance = Accounts.Sum(account => account.ClearedBalance);
        var assignedToCategoryGroups = CategoryGroups.Sum(cg => cg.Available);
        
        var result = availableAccountBalance - assignedToCategoryGroups;
        return [result];
    }
}