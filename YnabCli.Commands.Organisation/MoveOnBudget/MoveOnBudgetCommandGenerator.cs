using YnabCli.Commands.Generators;
using YnabCli.Instructions.Arguments;

namespace YnabCli.Commands.Organisation.MoveOnBudget;

public class MoveOnBudgetCommandGenerator : ICommandGenerator<MoveOnBudgetCommand>
{
    public ICommand Generate(string? subCommandName, List<InstructionArgument> arguments)
    {
        var accountIdArgument = arguments.OfRequiredType<Guid>(MoveOnBudgetCommand.ArgumentNames.AccountId);

        return new MoveOnBudgetCommand
        {
            AccountId = accountIdArgument.ArgumentValue
        };
    }
}