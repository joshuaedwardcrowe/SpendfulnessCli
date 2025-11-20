using Cli.Commands.Abstractions.Outcomes;
using Cli.Commands.Abstractions.Outcomes.Reusable;

namespace Cli.Commands.Abstractions.Properties;

public class ListAggregatorCliCommandPropertyFactory<TAggregate> : ICliCommandPropertyFactory
{
    public bool CanCreateProperty(CliCommandOutcome outcome)
    {
        return outcome is CliCommandListAggregatorOutcome<TAggregate>;
    }

    public CliCommandProperty CreateProperty(CliCommandOutcome outcome)
    {
        if (outcome is CliCommandListAggregatorOutcome<TAggregate> aggregatorOutcome)
        {
            return new ListAggregatorCliCommandProperty<TAggregate>(aggregatorOutcome.Aggregator);
        }
        
        throw new InvalidOperationException(
            $"Cannot create ListAggregatorCliCommandProperty<{typeof(TAggregate).Name}> from outcome of type {outcome.GetType().Name}");
    }
}