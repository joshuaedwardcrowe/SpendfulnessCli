using ConsoleTables;

namespace YnabCli.Commands.Personalisation.Commitments;

public class CommitmentCommandHandler : ICommandHandler<CommitmentCommand>
{
    public Task<ConsoleTable> Handle(CommitmentCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}