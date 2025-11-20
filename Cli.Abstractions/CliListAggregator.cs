namespace Cli.Abstractions;

public abstract class CliListAggregator<TAggregation> : CliAggregator<IEnumerable<TAggregation>>
{
    private readonly List<Func<IEnumerable<TAggregation>, IEnumerable<TAggregation>>> _aggregationOperationFunctions = [];

    protected CliListAggregator()
    {
    }

    public override IEnumerable<TAggregation> Aggregate()
    {
        var specificAggregation = ListAggregate();
        
        foreach (var aggregationOperationFunction in _aggregationOperationFunctions)
        {
            specificAggregation = aggregationOperationFunction(specificAggregation);
        }
        
        return specificAggregation;
    }
    
    public CliListAggregator<TAggregation> AfterAggregation(Func<IEnumerable<TAggregation>, IEnumerable<TAggregation>> operationFunction)
    {
        _aggregationOperationFunctions.Add(operationFunction);
        return this;
    }

    protected abstract IEnumerable<TAggregation> ListAggregate();
}