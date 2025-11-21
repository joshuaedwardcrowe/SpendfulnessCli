using Cli.Abstractions;
using Cli.Commands.Abstractions.Outcomes;
using Cli.Commands.Abstractions.Outcomes.Final;

namespace Cli.Commands.Abstractions.Handlers;

public abstract class CliCommandHandler
{
    protected static CliCommandOutcome[] OutcomeAs(CliTable cliTable)
        => [new CliCommandTableOutcome(cliTable)];

    protected static CliCommandOutcome[] OutcomeAs(string message)
        => [new CliCommandOutputOutcome(message)];

    protected static Task<CliCommandOutcome[]> AsyncOutcomeAs(string message)
        => Task.FromResult(OutcomeAs(message));
}