using Cli.Abstractions;

namespace Cli.Commands.Abstractions.Outcomes;

public class CliCommandAggregatorOutcome<TAggregate> : CliCommandOutcome
{
    public CliAggregator<TAggregate> Aggregator;
    
    public CliCommandAggregatorOutcome(CliAggregator<TAggregate> aggregator) : base(CliCommandOutcomeKind.Aggregate)
    {
        Aggregator = aggregator;
    }
    
}