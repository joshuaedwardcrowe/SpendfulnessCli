using Ynab;

namespace YnabCli.Aggregation.Aggregator.ListAggregators;

public abstract class ListAggregator<TAggregate> : Aggregator<IEnumerable<TAggregate>>
{
    private readonly List<Func<IEnumerable<TAggregate>, IEnumerable<TAggregate>>> _aggregationOperationFunctions = [];

    public ListAggregator()
    {
    }
    
    protected ListAggregator(IEnumerable<CategoryGroup> categoryGroups) : base(categoryGroups)
    {
    }

    protected ListAggregator(IEnumerable<CategoryGroup> categoryGroups, IEnumerable<Transaction> transactions) : base(categoryGroups, transactions)
    {
    }

    protected ListAggregator(IEnumerable<Transaction> transactions) : base(transactions)
    {
    }

    protected override IEnumerable<TAggregate> GenerateAggregate()
    {
        var specificAggregation = ListAggregate();
        
        foreach (var aggregationOperationFunction in _aggregationOperationFunctions)
        {
            specificAggregation = aggregationOperationFunction(specificAggregation);
        }
        
        return specificAggregation;
    }
    
    public ListAggregator<TAggregate> AfterAggregation(Func<IEnumerable<TAggregate>, IEnumerable<TAggregate>> operationFunction)
    {
        _aggregationOperationFunctions.Add(operationFunction);
        return this;
    }

    protected abstract IEnumerable<TAggregate> ListAggregate();
}