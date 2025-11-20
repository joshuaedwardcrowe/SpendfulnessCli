using Cli.Commands.Abstractions.Outcomes;
using Cli.Commands.Abstractions.Outcomes.Reusable;

namespace Cli.Commands.Abstractions.Properties;

public class AggregatorCliCommandPropertyFactory<TAggregate> : ICliCommandPropertyFactory
{
    public bool CanCreateProperty(CliCommandOutcome outcome)
    {
        return outcome is CliCommandAggregatorOutcome<TAggregate>;
    }

    public CliCommandProperty CreateProperty(CliCommandOutcome outcome)
    {
        if (outcome is CliCommandAggregatorOutcome<TAggregate> aggregatorOutcome)
        {
            return new AggregatorCliCommandProperty<TAggregate>(aggregatorOutcome.Aggregator);
        }
        
        throw new InvalidOperationException(
            $"Cannot create AggregatorCliCommandProperty<{typeof(TAggregate).Name}> from outcome of type {outcome.GetType().Name}");
    }
}