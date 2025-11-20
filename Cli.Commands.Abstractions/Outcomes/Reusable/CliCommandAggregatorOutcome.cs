using Cli.Abstractions;

namespace Cli.Commands.Abstractions.Outcomes.Reusable;

public class CliCommandAggregatorOutcome<TAggregate>(CliAggregator<TAggregate> aggregator)
    : CliCommandOutcome(CliCommandOutcomeKind.Reusable)
{
    public CliAggregator<TAggregate> Aggregator { get; } = aggregator;
}
public class CliCommandMessageOutcome(string message)
    : CliCommandOutcome(CliCommandOutcomeKind.Reusable)
{
    public string Message { get; } = message;
}