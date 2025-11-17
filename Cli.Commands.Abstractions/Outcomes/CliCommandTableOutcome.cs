using ConsoleTables;

namespace Cli.Commands.Abstractions.Outcomes;

public class CliCommandTableOutcome(ConsoleTable table) : CliCommandOutcome(CliCommandOutcomeKind.Table)
{
    public ConsoleTable Table = table;
}