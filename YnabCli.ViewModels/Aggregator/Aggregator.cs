using Ynab;

namespace YnabCli.ViewModels.Aggregator;

public abstract class Aggregator<TAggregation>
{
    protected IEnumerable<Account> Accounts { get; } 
    protected IEnumerable<CategoryGroup> CategoryGroups { get; }
    protected IEnumerable<Transaction> Transactions { get; }

    protected Aggregator()
    {
        Accounts = new List<Account>();
        CategoryGroups = new List<CategoryGroup>();
        Transactions = new List<Transaction>();
    }

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

    public abstract TAggregation Aggregate();
}