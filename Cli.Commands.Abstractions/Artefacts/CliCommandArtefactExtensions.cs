using Cli.Abstractions;
using Cli.Commands.Abstractions.Artefacts.Aggregator;
using Cli.Commands.Abstractions.Artefacts.CommandRan;

namespace Cli.Commands.Abstractions.Artefacts;

public static class CliCommandArtefactExtensions
{
    public static bool LastCommandRanWas<TCliCommand>(this List<CliCommandArtefact> properties)
        where TCliCommand : CliCommand
    {
        var lastCommandRanProperty = properties.OfType<CliCommandRanArtefact>().LastOrDefault();
        if (lastCommandRanProperty == null)
        { 
            return false;
        }
        
        return lastCommandRanProperty.RanCommand is TCliCommand;
    }

    private static List<ListAggregatorCliCommandArtefact<TAggregate>> OfListAggregatorType<TAggregate>(
        this List<CliCommandArtefact> properties)
            => properties
                .OfType<ListAggregatorCliCommandArtefact<TAggregate>>()
                .ToList();

    public static CliListAggregator<TAggregate>? GetListAggregator<TAggregate>(
        this List<CliCommandArtefact> properties)
            => properties
            .OfListAggregatorType<TAggregate>()
            .FirstOrDefault()
            ?.Value;
}