using Ynab;

namespace YnabCli.ViewModels.Aggregator;

public abstract class ListAggregator<TAggregate> : Aggregator<IEnumerable<TAggregate>>
{
    private readonly List<Func<IEnumerable<Transaction>, IEnumerable<Transaction>>> _transactionOperationFunctions = [];
    private readonly List<Func<IEnumerable<TAggregate>, IEnumerable<TAggregate>>> _aggregationOperationFunctions = [];

    protected ListAggregator(IEnumerable<Transaction> transactions)
        : base(transactions)
    {
    }

    public override IEnumerable<TAggregate> Aggregate()
    {
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

    public ListAggregator<TAggregate> WhereTransactions(Func<IEnumerable<Transaction>, IEnumerable<Transaction>> operationFunction)
    {
        _transactionOperationFunctions.Add(operationFunction);
        return this;
    }

    public ListAggregator<TAggregate> WhereAggregates(Func<IEnumerable<TAggregate>, IEnumerable<TAggregate>> operationFunction)
    {
        _aggregationOperationFunctions.Add(operationFunction);
        return this;
    }

    protected abstract IEnumerable<TAggregate> ListAggregate();
}