using Cli.Commands.Abstractions.Outcomes;
using Cli.Commands.Abstractions.Outcomes.Final;

namespace Cli.Commands.Abstractions.Artefacts.Aggregator.Filters;

public class CliListAggregatorFilterCliCommandArtefactFactory : ICliCommandArtefactFactory
{
    public bool For(CliCommandOutcome outcome) => outcome is FilterCliCommandOutcome;

    public CliCommandArtefact Create(CliCommandOutcome outcome)
    {
        var filteredOutcome = (FilterCliCommandOutcome)outcome;
        return new CliListAggregatorFilterCliCommandArtefact(filteredOutcome.CliListAggregatorFilter);
    }
}