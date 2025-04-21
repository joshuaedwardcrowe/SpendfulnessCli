using Ynab;

namespace YnabCli.Aggregation.Aggregator.ListAggregators;

public abstract class ListAggregator<TAggregate> : Aggregator<IEnumerable<TAggregate>>
{
    private readonly List<Func<IEnumerable<Account>, IEnumerable<Account>>> _accountOperationFunctions = [];
    private readonly List<Func<IEnumerable<Transaction>, IEnumerable<Transaction>>> _transactionOperationFunctions = [];
    private readonly List<Func<IEnumerable<TAggregate>, IEnumerable<TAggregate>>> _aggregationOperationFunctions = [];

    public ListAggregator()
    {
    }
    
    protected ListAggregator(IEnumerable<CategoryGroup> categoryGroups) : base(categoryGroups)
    {
    }

    protected ListAggregator(IEnumerable<Transaction> transactions) : base(transactions)
    {
    }

    public override IEnumerable<TAggregate> Aggregate()
    {
        foreach (var accountOperationFunction in _accountOperationFunctions)
        {
            Accounts = accountOperationFunction(Accounts);
        }
        
        foreach (var transactionOperationFunction in _transactionOperationFunctions)
        {
            Transactions = transactionOperationFunction(Transactions);
        }
        
        var specificAggregation = ListAggregate();
        
        foreach (var aggregationOperationFunction in _aggregationOperationFunctions)
        {
            specificAggregation = aggregationOperationFunction(specificAggregation);
        }
        
        return specificAggregation;
    }

    public ListAggregator<TAggregate> BeforeAggregation(Func<IEnumerable<Transaction>, IEnumerable<Transaction>> operationFunction)
    {
        _transactionOperationFunctions.Add(operationFunction);
        return this;
    }

    public ListAggregator<TAggregate> BeforeAggregation(Func<IEnumerable<Account>, IEnumerable<Account>> operationFunction)
    {
        _accountOperationFunctions.Add(operationFunction);
        return this;
    }

    public ListAggregator<TAggregate> AfterAggregation(Func<IEnumerable<TAggregate>, IEnumerable<TAggregate>> operationFunction)
    {
        _aggregationOperationFunctions.Add(operationFunction);
        return this;
    }

    protected abstract IEnumerable<TAggregate> ListAggregate();
}