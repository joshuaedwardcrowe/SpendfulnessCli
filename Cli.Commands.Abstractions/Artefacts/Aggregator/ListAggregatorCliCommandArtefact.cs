using Cli.Abstractions.Aggregators;

namespace Cli.Commands.Abstractions.Artefacts.Aggregator;

public class ListAggregatorCliCommandArtefact<TAggregate>(CliListAggregator<TAggregate> artefactValue)
    : ValuedCliCommandArtefact<CliListAggregator<TAggregate>>(artefactValue)
{
    
}