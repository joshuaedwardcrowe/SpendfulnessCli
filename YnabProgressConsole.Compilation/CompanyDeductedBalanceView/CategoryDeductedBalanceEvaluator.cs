using Ynab;

namespace YnabProgressConsole.Compilation.SpareMoneyView;

public class CategoryDeductedBalanceEvaluator : YnabEvaluator<decimal>
{
    public CategoryDeductedBalanceEvaluator(
        IEnumerable<Account>? accounts = null, 
        IEnumerable<CategoryGroup>? categoryGroups = null, 
        IEnumerable<Category>? categories = null, 
        IEnumerable<ScheduledTransaction>? scheduledTransactions = null, 
        IEnumerable<Transaction>? transactions = null) :
        base(accounts, categoryGroups, categories, scheduledTransactions, transactions)
    {
    }

    public override decimal Evaluate()
    {
        var availableAccountBalance = Accounts.Sum(account => account.ClearedBalance);
        var assignedToCategoryGroups = CategoryGroups.Sum(cg => cg.Available);
        
        return availableAccountBalance - assignedToCategoryGroups;
    }
}
