using Ynab;

namespace YnabProgressConsole.Compilation.Evaluators;

public class CategoryDeductedBalanceAggregator(List<Account> accounts, List<CategoryGroup> categoryGroups)
    : Aggregator<decimal>(accounts, categoryGroups)
{
    public override decimal Evaluate()
    {
        var availableAccountBalance = Accounts.Sum(account => account.ClearedBalance);
        var assignedToCategoryGroups = CategoryGroups.Sum(cg => cg.Available);
        
        return availableAccountBalance - assignedToCategoryGroups;
    }
}
