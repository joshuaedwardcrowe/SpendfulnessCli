using Cli.Abstractions;
using Cli.Abstractions.Aggregators;
using Spendfulness.Database.Commitments;
using Ynab;

namespace SpendfulnessCli.Aggregation.Aggregator;

// TODO: Renamed to YnabPagedListAggregator
public abstract class YnabListAggregator<TAggregation> : CliListAggregator<TAggregation>
{
    protected IEnumerable<Account> Accounts { get; set; } 
    protected IEnumerable<CategoryGroup> CategoryGroups { get; set; }
    protected IEnumerable<Transaction> Transactions { get; set; }
    protected ICollection<Commitment> Commitments { get; }
    
    private readonly List<Func<IEnumerable<Account>, IEnumerable<Account>>> _accountOperationFunctions = [];
    private readonly List<Func<IEnumerable<CategoryGroup>, IEnumerable<CategoryGroup>>> _categoryGroupOperations = [];
    private readonly List<Func<IEnumerable<Transaction>, IEnumerable<Transaction>>> _transactionOperationFunctions = [];

    protected YnabListAggregator()
    {
        Accounts = new List<Account>();
        CategoryGroups = new List<CategoryGroup>();
        Transactions = new List<Transaction>();
        Commitments = new List<Commitment>();
    }

    protected YnabListAggregator(IEnumerable<Account> accounts, IEnumerable<CategoryGroup> categoryGroups)
    {
        Accounts = accounts;
        CategoryGroups = categoryGroups;
        Transactions = new List<Transaction>();
        Commitments = new List<Commitment>();
    }

    protected YnabListAggregator(IEnumerable<CategoryGroup> categoryGroups, IEnumerable<Transaction> transactions)
    {
        Accounts = new List<Account>();
        CategoryGroups = categoryGroups;
        Transactions = transactions;
        Commitments = new List<Commitment>();
    }

    protected YnabListAggregator(IEnumerable<CategoryGroup> categoryGroups)
    {
        Accounts = new List<Account>();
        CategoryGroups = categoryGroups;
        Transactions = new List<Transaction>();
        Commitments = new List<Commitment>();
    }

    protected YnabListAggregator(IEnumerable<Transaction> transactions, IEnumerable<CategoryGroup> categoryGroups)
    {
        Accounts = new List<Account>();
        Transactions = transactions;
        CategoryGroups = categoryGroups;
        Commitments = new List<Commitment>();
    }

    protected YnabListAggregator(IEnumerable<Transaction> transactions)
    {
        Accounts = new List<Account>();
        CategoryGroups = new List<CategoryGroup>();
        Transactions = transactions;
        Commitments = new List<Commitment>();
    }

    protected YnabListAggregator(IEnumerable<Transaction> transactions, int pageSize, int pageNumber) : base(pageSize, pageNumber)
    {
        Accounts = new List<Account>();
        CategoryGroups = new List<CategoryGroup>();
        Transactions = transactions;
        Commitments = new List<Commitment>();
    }

    protected YnabListAggregator(ICollection<Commitment> commitments)
    {
        Accounts = new List<Account>();
        CategoryGroups = new List<CategoryGroup>();
        Transactions = new List<Transaction>();
        Commitments = commitments;
    }

    protected abstract IEnumerable<TAggregation> GenerateAggregate();

    protected override IEnumerable<TAggregation> ListAggregate()
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
    
    public YnabListAggregator<TAggregation> BeforeAggregation(Func<IEnumerable<Transaction>, IEnumerable<Transaction>> operationFunction)
    {
        _transactionOperationFunctions.Add(operationFunction);
        return this;
    }

    public YnabListAggregator<TAggregation> BeforeAggregation(Func<IEnumerable<CategoryGroup>, IEnumerable<CategoryGroup>> operationFunction)
    {
        _categoryGroupOperations.Add(operationFunction);
        return this;
    }

    public YnabListAggregator<TAggregation> BeforeAggregation(Func<IEnumerable<Account>, IEnumerable<Account>> operationFunction)
    {
        _accountOperationFunctions.Add(operationFunction);
        return this;
    }
}