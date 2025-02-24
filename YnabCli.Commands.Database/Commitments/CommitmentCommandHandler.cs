using ConsoleTables;

namespace YnabCli.Commands.Database.Commitments;

public class CommitmentCommandHandler : ICommandHandler<CommitmentCommand>
{
    public Task<ConsoleTable> Handle(CommitmentCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}