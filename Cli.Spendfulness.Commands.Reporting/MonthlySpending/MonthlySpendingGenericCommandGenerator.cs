using Cli.Commands.Abstractions;
using Cli.Instructions.Abstractions;
using Cli.Instructions.Arguments;

namespace Cli.Ynab.Commands.Reporting.MonthlySpending;

public class MonthlySpendingGenericCommandGenerator : ICommandGenerator<MonthlySpendingCommand>
{
    public ICommand Generate(string? subCommandName, List<ConsoleInstructionArgument> arguments)
    {
        var categoryIdArgument = arguments.OfType<Guid>(MonthlySpendingCommand.ArgumentNames.CategoryId);

        return new MonthlySpendingCommand
        {
            CategoryId = categoryIdArgument?.ArgumentValue
        };
    }
}