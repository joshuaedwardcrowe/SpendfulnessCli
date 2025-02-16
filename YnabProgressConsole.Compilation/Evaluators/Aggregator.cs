using Ynab;

namespace YnabProgressConsole.Compilation.Evaluators;

public abstract class Aggregator<TEvaluation>
{
    protected IEnumerable<Account> Accounts { get; } 
    protected IEnumerable<CategoryGroup> CategoryGroups { get; }
    protected IEnumerable<Transaction> Transactions { get; }

    protected Aggregator(IEnumerable<Account> accounts, IEnumerable<CategoryGroup> categoryGroups)
    {
        Accounts = accounts;
        CategoryGroups = categoryGroups;
        Transactions = new List<Transaction>();
    }

    protected Aggregator(IEnumerable<CategoryGroup> categoryGroups, IEnumerable<Transaction> transactions)
    {
        Accounts = new List<Account>();
        CategoryGroups = categoryGroups;
        Transactions = transactions;
    }

    protected Aggregator(IEnumerable<Transaction> transactions)
    {
        Accounts = new List<Account>();
        CategoryGroups = new List<CategoryGroup>();
        Transactions = transactions;
    }

    public abstract TEvaluation Evaluate();
}