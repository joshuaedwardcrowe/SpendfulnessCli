using ConsoleTables;
using YnabCli.Commands.Handlers;

namespace YnabCli.Commands.Personalisation.Commitments.Find;

public class CommitmentFindCommandHandler : CommandHandler, ICommandHandler<CommitmentFindCommand>
{
    public async Task<ConsoleTable> Handle(CommitmentFindCommand request, CancellationToken cancellationToken)
    {
        await Task.Delay(0, cancellationToken);
        return CompileMessage("This is a message");
    }
}