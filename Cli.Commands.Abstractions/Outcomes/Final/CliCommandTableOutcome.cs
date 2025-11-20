using Cli.Abstractions;

namespace Cli.Commands.Abstractions.Outcomes.Final;

public class CliCommandTableOutcome(CliTable table) : CliCommandOutcome(CliCommandOutcomeKind.Final)
{
    public CliTable Table = table;
}