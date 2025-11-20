using Cli.Abstractions;

namespace Cli.Commands.Abstractions.Outcomes;

public class CliCommandTableOutcome(CliTable table) : CliCommandOutcome
{
    public CliTable Table = table;
}