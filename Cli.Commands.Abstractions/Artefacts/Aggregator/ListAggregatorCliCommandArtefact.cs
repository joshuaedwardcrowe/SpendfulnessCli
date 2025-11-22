using Cli.Abstractions;

namespace Cli.Commands.Abstractions.Artefacts.Aggregator;

public class ListAggregatorCliCommandArtefact<TAggregate>(CliListAggregator<TAggregate> value)
    : ValuedCliCommandArtefact<CliListAggregator<TAggregate>>(value)
{
    
}