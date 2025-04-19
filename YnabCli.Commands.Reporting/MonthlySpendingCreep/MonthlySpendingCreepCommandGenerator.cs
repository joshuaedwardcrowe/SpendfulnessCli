using YnabCli.Commands.Generators;
using YnabCli.Instructions.Arguments;

namespace YnabCli.Commands.Reporting.MonthlySpendingCreep;

public class MonthlySpendingCreepCommandGenerator : ICommandGenerator, ITypedCommandGenerator<MonthlySpendingCreepCommand>
{
    public ICommand Generate(string? subCommandName, List<InstructionArgument> arguments)
    {
        return new MonthlySpendingCreepCommand();
    }
}