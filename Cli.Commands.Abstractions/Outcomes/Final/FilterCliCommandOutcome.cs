using Cli.Abstractions.Aggregators.Filters;

namespace Cli.Commands.Abstractions.Outcomes.Final;

public class FilterCliCommandOutcome : CliCommandOutcome
{
    public FilterCliCommandOutcome(CliListAggregatorFilter cliListAggregatorFilter) : base(CliCommandOutcomeKind.Reusable)
    {
        CliListAggregatorFilter = cliListAggregatorFilter;
    }

    public CliListAggregatorFilter CliListAggregatorFilter { get; }
}