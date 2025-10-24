using Ynab;

namespace Cli.Spendfulness.Aggregation.Aggregator.ListAggregators;

public abstract class ListYnabAggregator<TAggregate> : YnabAggregator<IEnumerable<TAggregate>>
{
    private readonly List<Func<IEnumerable<TAggregate>, IEnumerable<TAggregate>>> _aggregationOperationFunctions = [];

    protected ListYnabAggregator()
    {
    }
    
    protected ListYnabAggregator(IEnumerable<CategoryGroup> categoryGroups) : base(categoryGroups)
    {
    }

    protected ListYnabAggregator(IEnumerable<CategoryGroup> categoryGroups, IEnumerable<Transaction> transactions) : base(categoryGroups, transactions)
    {
    }

    protected ListYnabAggregator(IEnumerable<Transaction> transactions) : base(transactions)
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
    
    public ListYnabAggregator<TAggregate> AfterAggregation(Func<IEnumerable<TAggregate>, IEnumerable<TAggregate>> operationFunction)
    {
        _aggregationOperationFunctions.Add(operationFunction);
        return this;
    }

    protected abstract IEnumerable<TAggregate> ListAggregate();
}