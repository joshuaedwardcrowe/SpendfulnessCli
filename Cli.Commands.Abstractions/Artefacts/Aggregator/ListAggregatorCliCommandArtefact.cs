using Cli.Abstractions.Aggregators;

namespace Cli.Commands.Abstractions.Artefacts.Aggregator;

public class ListAggregatorCliCommandArtefact<TAggregate>(CliListAggregator<TAggregate> artefactArtefactValue)
    : ValuedCliCommandArtefact<CliListAggregator<TAggregate>>(typeof(TAggregate).Name, artefactArtefactValue)
{
    
}