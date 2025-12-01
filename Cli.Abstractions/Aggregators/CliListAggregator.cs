namespace Cli.Abstractions.Aggregators;

public abstract class CliListAggregator<TAggregate> : CliAggregator<IEnumerable<TAggregate>>
{
    private readonly int _pageSize;
    private readonly int _pageNumber;
    
    private readonly List<Func<IEnumerable<TAggregate>, IEnumerable<TAggregate>>> _aggregateFunctions = [];

    protected CliListAggregator(int? pageSize = null, int? pageNumber = null)
    {
        _pageSize = pageSize ?? 20;
        _pageNumber = pageNumber ?? 1;
    }
    
    public override IEnumerable<TAggregate> Aggregate()
    {
        var aggregates = ListAggregate();

        var skipNumber = _pageSize * (_pageNumber - 1);
        
        aggregates = aggregates.Skip(skipNumber);
        
        aggregates = aggregates.Take(_pageSize);

        return _aggregateFunctions.Aggregate(aggregates, ApplyAggregateFunction);
    }
    public CliListAggregator<TAggregate> AfterAggregation(Func<IEnumerable<TAggregate>, IEnumerable<TAggregate>> operationFunction)
    {
        _aggregateFunctions.Add(operationFunction);
        return this;
    }

    protected abstract IEnumerable<TAggregate> ListAggregate();
    
    private IEnumerable<TAggregate> ApplyAggregateFunction(IEnumerable<TAggregate> current, Func<IEnumerable<TAggregate>, IEnumerable<TAggregate>> operationFunction) => operationFunction(current);

}