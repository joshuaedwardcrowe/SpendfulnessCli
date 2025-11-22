using Cli.Commands.Abstractions.Outcomes;
using Cli.Commands.Abstractions.Outcomes.Final;

namespace Cli.Commands.Abstractions.Handlers;

public abstract class NoCliCommandHandler<TCommand> : CliCommandHandler, ICliCommandHandler<TCommand> where TCommand : CliCommand
{
    public Task<CliCommandOutcome[]> Handle(TCommand command, CancellationToken cancellationToken)
        => AsyncOutcomeAs(
            $"No functionality for {command.GetSpecificCommandName()} base command");
}