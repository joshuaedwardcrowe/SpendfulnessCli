using YnabCli.Commands.Generators;
using YnabCli.Instructions.Arguments;

namespace YnabCli.Commands.Reporting.MonthlySpending;

public class MonthlySpendingCommandGenerator : ICommandGenerator, ITypedCommandGenerator<MonthlySpendingCommand>
{
    public ICommand Generate(string? subCommandName, List<InstructionArgument> arguments)
    {
        var categoryIdArgument = arguments.OfType<Guid>(MonthlySpendingCommand.ArgumentNames.CategoryId);

        return new MonthlySpendingCommand
        {
            CategoryId = categoryIdArgument?.ArgumentValue
        };
    }
}