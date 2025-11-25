using Cli.Abstractions.Aggregators.Filters;

namespace Cli.Commands.Abstractions.Artefacts.Aggregator.Filters;

public class CliListAggregatorFilterCliCommandArtefact(CliListAggregatorFilter cliListAggregatorFilter) : CliCommandArtefact
{
    public CliListAggregatorFilter CliListAggregatorFilter { get; } = cliListAggregatorFilter;
}