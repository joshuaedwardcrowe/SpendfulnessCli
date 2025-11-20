using Cli.Abstractions;
using Cli.Commands.Abstractions.Outcomes;
using ConsoleTables;

namespace Cli.Commands.Abstractions;

public abstract class CliCommandHandler
{
    protected static CliCommandOutcome[] OutcomeAs(CliTable cliTable)
    {
        var outcome = new CliCommandTableOutcome(cliTable);

        return [outcome];
    }

    protected static CliCommandOutcome[] OutcomeAs(string message)
    {
        var outcome = new CliCommandOutputOutcome(message);

        return [outcome];
    }
}