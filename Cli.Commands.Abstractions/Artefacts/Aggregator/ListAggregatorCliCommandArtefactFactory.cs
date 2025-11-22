using Cli.Commands.Abstractions.Outcomes;
using Cli.Commands.Abstractions.Outcomes.Reusable;

namespace Cli.Commands.Abstractions.Artefacts.Aggregator;

public class ListAggregatorCliCommandArtefactFactory<TAggregate> : ICliCommandArtefactFactory
{
    public bool For(CliCommandOutcome outcome) => outcome is CliCommandListAggregatorOutcome<TAggregate>;

    public CliCommandArtefact Create(CliCommandOutcome outcome)
    {
        if (outcome is CliCommandListAggregatorOutcome<TAggregate> aggregatorOutcome)
        {
            return new ListAggregatorCliCommandArtefact<TAggregate>(aggregatorOutcome.Aggregator);
        }
        
        throw new InvalidOperationException(
            $"Cannot create ListAggregatorCliCommandProperty<{typeof(TAggregate).Name}> from outcome of type {outcome.GetType().Name}");
    }
}