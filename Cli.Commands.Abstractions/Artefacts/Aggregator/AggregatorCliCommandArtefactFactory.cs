using Cli.Commands.Abstractions.Outcomes;
using Cli.Commands.Abstractions.Outcomes.Reusable;

namespace Cli.Commands.Abstractions.Artefacts.Aggregator;

public class AggregatorCliCommandArtefactFactory<TAggregate> : ICliCommandArtefactFactory
{
    public bool CanCreateWhen(CliCommandOutcome outcome)
    {
        return outcome is CliCommandAggregatorOutcome<TAggregate>;
    }

    public CliCommandArtefact Create(CliCommandOutcome outcome)
    {
        if (outcome is CliCommandAggregatorOutcome<TAggregate> aggregatorOutcome)
        {
            return new AggregatorCliCommandArtefact<TAggregate>(aggregatorOutcome.Aggregator);
        }
        
        throw new InvalidOperationException(
            $"Cannot create AggregatorCliCommandProperty<{typeof(TAggregate).Name}> from outcome of type {outcome.GetType().Name}");
    }
}