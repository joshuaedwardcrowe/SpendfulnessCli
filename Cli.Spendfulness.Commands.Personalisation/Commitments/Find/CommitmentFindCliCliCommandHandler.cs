using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Outcomes;

namespace Cli.Spendfulness.Commands.Personalisation.Commitments.Find;

public class CommitmentFindCliCliCommandHandler : CliCommandHandler, ICliCommandHandler<CommitmentFindCliCommand>
{
    public async Task<CliCommandOutcome> Handle(CommitmentFindCliCommand request, CancellationToken cancellationToken)
    {
        await Task.Delay(0, cancellationToken);
        return Compile("This is a message");
    }
}