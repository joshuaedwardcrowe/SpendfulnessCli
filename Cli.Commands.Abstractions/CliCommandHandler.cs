using Cli.Abstractions;
using Cli.Commands.Abstractions.Outcomes;
using ConsoleTables;

namespace Cli.Commands.Abstractions;

public abstract class CliCommandHandler
{
    protected static CliCommandTableOutcome Compile(CliTable cliTable)
    {
        return new CliCommandTableOutcome(cliTable);
    }

    protected static CliCommandOutputOutcome Compile(string message) => new(message);
}