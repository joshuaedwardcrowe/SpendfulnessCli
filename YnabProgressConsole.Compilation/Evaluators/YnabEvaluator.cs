using Ynab;

namespace YnabProgressConsole.Compilation.Evaluators;

public abstract class YnabEvaluator<TEvaluation>
{
    protected IEnumerable<Account> Accounts { get; } 
    protected IEnumerable<CategoryGroup> CategoryGroups { get; }
    protected IEnumerable<Transaction> Transactions { get; }

    protected YnabEvaluator(IEnumerable<Account> accounts, IEnumerable<CategoryGroup> categoryGroups)
    {
        Accounts = accounts;
        CategoryGroups = categoryGroups;
        Transactions = new List<Transaction>();
    }

    protected YnabEvaluator(IEnumerable<Transaction> transactions)
    {
        Accounts = new List<Account>();
        CategoryGroups = new List<CategoryGroup>();
        Transactions = transactions;
    }

    public abstract TEvaluation Evaluate();
}