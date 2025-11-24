using Cli.Abstractions;
using Cli.Abstractions.Aggregators;

namespace Cli.Commands.Abstractions.Artefacts.Aggregator;

public class AggregatorCliCommandArtefact<TAggregate>(CliAggregator<TAggregate> artefactValue)
    : ValuedCliCommandArtefact<CliAggregator<TAggregate>>(artefactValue)
{
    
}