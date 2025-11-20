using Cli.Commands.Abstractions.Outcomes;
using Cli.Commands.Abstractions.Outcomes.Final;

namespace Cli.Commands.Abstractions.Handlers;

// TODO: Add unit tests.
public abstract class NoCliCommandHandler<TCommand> : ICliCommandHandler<TCommand> where TCommand : CliCommand
{
    public Task<CliCommandOutcome[]> Handle(TCommand command, CancellationToken cancellationToken)
    {
        var outcome = new CliCommandOutputOutcome($"No functionality for {command.GetCommandName()} base command");
        return Task.FromResult<CliCommandOutcome[]>([outcome]);
    }
}