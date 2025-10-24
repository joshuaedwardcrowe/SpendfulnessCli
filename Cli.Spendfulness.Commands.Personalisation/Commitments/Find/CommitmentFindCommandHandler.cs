using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Outcomes;
using ConsoleTables;

namespace Cli.Spendfulness.Commands.Personalisation.Commitments.Find;

public class CommitmentFindCommandHandler : CommandHandler, ICommandHandler<CommitmentFindCommand>
{
    public async Task<CliCommandOutcome> Handle(CommitmentFindCommand request, CancellationToken cancellationToken)
    {
        await Task.Delay(0, cancellationToken);
        return Compile("This is a message");
    }
}