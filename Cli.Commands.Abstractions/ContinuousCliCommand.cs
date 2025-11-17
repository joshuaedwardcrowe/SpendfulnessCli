using Cli.Abstractions;

namespace Cli.Commands.Abstractions;

public record ContinuousCliCommand<TAggregate> : CliCommand
{
    public CliAggregator<TAggregate> Aggregator { get; }
    
    public ContinuousCliCommand(CliAggregator<TAggregate> aggregator)
    {
        Aggregator = aggregator;
    }
}