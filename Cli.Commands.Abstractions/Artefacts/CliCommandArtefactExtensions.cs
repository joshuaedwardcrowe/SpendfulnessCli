using Cli.Abstractions;
using Cli.Abstractions.Aggregators;
using Cli.Commands.Abstractions.Artefacts.Aggregator;
using Cli.Commands.Abstractions.Artefacts.CommandRan;

namespace Cli.Commands.Abstractions.Artefacts;

public static class CliCommandArtefactExtensions
{
    public static bool LastCommandRanWas<TCliCommand>(this List<CliCommandArtefact> artefacts)
        where TCliCommand : CliCommand
    {
        var lastCommandRanProperty = artefacts
            .OfType<CliCommandRanArtefact>()
            .LastOrDefault();

        return lastCommandRanProperty?.RanCommand is TCliCommand;
    }

    private static List<ListAggregatorCliCommandArtefact<TAggregate>> OfListAggregatorType<TAggregate>(
        this List<CliCommandArtefact> artefacts)
            => artefacts
                .OfType<ListAggregatorCliCommandArtefact<TAggregate>>()
                .ToList();

    public static CliListAggregator<TAggregate>? GetListAggregator<TAggregate>(
        this List<CliCommandArtefact> artefacts)
            => artefacts
                .OfListAggregatorType<TAggregate>()
                .FirstOrDefault()
                ?.Value;
}