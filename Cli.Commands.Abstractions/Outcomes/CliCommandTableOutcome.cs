using ConsoleTables;

namespace Cli.Commands.Abstractions.Outcomes;

public class CliCommandTableOutcome(ConsoleTable table) : CliCommandOutcome
{
    public ConsoleTable Table = table;
}