using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Outcomes;

namespace SpendfulnessCli.Commands.Personalisation.Commitments.Find;

public class CommitmentFindCliCommandHandler : CliCommandHandler, ICliCommandHandler<CommitmentFindCliCommand>
{
    public async Task<CliCommandOutcome[]> Handle(CommitmentFindCliCommand request, CancellationToken cancellationToken)
    {
        await Task.Delay(0, cancellationToken);
        return OutcomeAs("This is a message");
    }
}