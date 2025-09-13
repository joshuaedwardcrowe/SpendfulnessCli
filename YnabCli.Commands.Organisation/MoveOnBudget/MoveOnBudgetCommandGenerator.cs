using YnabCli.Commands.Generators;
using YnabCli.Instructions.Arguments;

namespace YnabCli.Commands.Organisation.MoveOnBudget;

public class MoveOnBudgetCommandGenerator : ICommandGenerator<MoveOnBudgetCommand>
{
    public ICommand Generate(string? subCommandName, List<InstructionArgument> arguments)
    {
        return new MoveOnBudgetCommand();
    }
}