using Ynab;
using YnabCli.Database.Commitments;

namespace YnabCli.Aggregation.Aggregator;

// TODO: I dont like that this needs to depend on the entire YNAB project for these models.
public abstract class Aggregator<TAggregation>
{
    protected IEnumerable<Account> Accounts { get; set; } 
    protected IEnumerable<CategoryGroup> CategoryGroups { get; }
    protected IEnumerable<Transaction> Transactions { get; set; }
    protected ICollection<Commitment> Commitments { get; }

    protected Aggregator()
    {
        Accounts = new List<Account>();
        CategoryGroups = new List<CategoryGroup>();
        Transactions = new List<Transaction>();
        Commitments = new List<Commitment>();
    }

    protected Aggregator(IEnumerable<Account> accounts, IEnumerable<CategoryGroup> categoryGroups)
    {
        Accounts = accounts;
        CategoryGroups = categoryGroups;
        Transactions = new List<Transaction>();
        Commitments = new List<Commitment>();
    }

    protected Aggregator(IEnumerable<CategoryGroup> categoryGroups, IEnumerable<Transaction> transactions)
    {
        Accounts = new List<Account>();
        CategoryGroups = categoryGroups;
        Transactions = transactions;
        Commitments = new List<Commitment>();
    }

    protected Aggregator(IEnumerable<Transaction> transactions)
    {
        Accounts = new List<Account>();
        CategoryGroups = new List<CategoryGroup>();
        Transactions = transactions;
        Commitments = new List<Commitment>();
    }

    protected Aggregator(ICollection<Commitment> commitments)
    {
        Accounts = new List<Account>();
        CategoryGroups = new List<CategoryGroup>();
        Transactions = new List<Transaction>();
        Commitments = commitments;
    }

    public abstract TAggregation Aggregate();
}