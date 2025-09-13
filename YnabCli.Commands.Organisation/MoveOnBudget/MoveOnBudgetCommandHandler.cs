using ConsoleTables;
using YnabCli.Commands.Handlers;

namespace YnabCli.Commands.Organisation.MoveOnBudget;

public class MoveOnBudgetCommandHandler : ICommandHandler<MoveOnBudgetCommand>
{
    public Task<ConsoleTable> Handle(MoveOnBudgetCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}