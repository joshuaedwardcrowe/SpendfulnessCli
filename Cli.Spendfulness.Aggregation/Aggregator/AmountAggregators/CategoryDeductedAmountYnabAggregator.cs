using Ynab;

namespace Cli.Spendfulness.Aggregation.Aggregator.AmountAggregators;

public class CategoryDeductedAmountYnabAggregator(IEnumerable<Account> accounts, IEnumerable<CategoryGroup> categoryGroups)
    : YnabAggregator<decimal>(accounts, categoryGroups)
{
    protected override decimal GenerateAggregate()
    {
        var availableAccountBalance = Accounts.Sum(account => account.ClearedBalance);
        var assignedToCategoryGroups = CategoryGroups.Sum(cg => cg.Available);
        
        return availableAccountBalance - assignedToCategoryGroups;
    }
}
