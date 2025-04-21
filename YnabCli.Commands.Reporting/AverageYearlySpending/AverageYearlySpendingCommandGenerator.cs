using YnabCli.Commands.Generators;
using YnabCli.Instructions.Arguments;

namespace YnabCli.Commands.Reporting.AverageYearlySpending;

public class AverageYearlySpendingCommandGenerator : ICommandGenerator, ITypedCommandGenerator<AverageYearlySpendingCommand>
{
    public ICommand Generate(string? subCommandName, List<InstructionArgument> arguments)
    {
        return new AverageYearlySpendingCommand();
    }
}