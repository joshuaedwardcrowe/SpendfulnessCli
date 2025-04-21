using Ynab;
using YnabCli.Database.Commitments;

namespace YnabCli.Aggregation.Aggregator;

// TODO: I dont like that this needs to depend on the entire YNAB project for these models.
public abstract class Aggregator<TAggregation>
{
    protected IEnumerable<Account> Accounts { get; set; } 
    protected IEnumerable<CategoryGroup> CategoryGroups { get; set; }
    protected IEnumerable<Transaction> Transactions { get; set; }
    protected ICollection<Commitment> Commitments { get; }
    
    private readonly List<Func<IEnumerable<Account>, IEnumerable<Account>>> _accountOperationFunctions = [];
    private readonly List<Func<IEnumerable<CategoryGroup>, IEnumerable<CategoryGroup>>> _categoryGroupOperations = [];
    private readonly List<Func<IEnumerable<Transaction>, IEnumerable<Transaction>>> _transactionOperationFunctions = [];

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

    protected Aggregator(IEnumerable<CategoryGroup> categoryGroups)
    {
        Accounts = new List<Account>();
        CategoryGroups = categoryGroups;
        Transactions = new List<Transaction>();
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

    protected abstract TAggregation GenerateAggregate();

    public TAggregation Aggregate()
    {
        foreach (var accountOperationFunction in _accountOperationFunctions)
        {
            Accounts = accountOperationFunction(Accounts);
        }

        foreach (var categoryGroupOperation in _categoryGroupOperations)
        {
            CategoryGroups = categoryGroupOperation(CategoryGroups);
        }
        
        foreach (var transactionOperationFunction in _transactionOperationFunctions)
        {
            Transactions = transactionOperationFunction(Transactions);
        }
        
        return GenerateAggregate();
    }
    
    public Aggregator<TAggregation> BeforeAggregation(Func<IEnumerable<Transaction>, IEnumerable<Transaction>> operationFunction)
    {
        _transactionOperationFunctions.Add(operationFunction);
        return this;
    }

    public Aggregator<TAggregation> BeforeAggregation(Func<IEnumerable<CategoryGroup>, IEnumerable<CategoryGroup>> operationFunction)
    {
        _categoryGroupOperations.Add(operationFunction);
        return this;
    }

    public Aggregator<TAggregation> BeforeAggregation(Func<IEnumerable<Account>, IEnumerable<Account>> operationFunction)
    {
        _accountOperationFunctions.Add(operationFunction);
        return this;
    }
}