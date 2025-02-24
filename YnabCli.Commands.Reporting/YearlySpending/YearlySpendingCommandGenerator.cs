using YnabCli.Instructions.Arguments;

namespace YnabCli.Commands.Reporting.YearlySpending;

public class YearlySpendingCommandGenerator : ICommandGenerator, ITypedCommandGenerator<YearlySpendingCommand>
{
    public ICommand Generate(string? subCommandName, List<InstructionArgument> arguments) => new YearlySpendingCommand();
}