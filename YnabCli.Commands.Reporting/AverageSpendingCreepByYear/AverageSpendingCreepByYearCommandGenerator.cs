using YnabCli.Commands.Generators;
using YnabCli.Instructions.Arguments;

namespace YnabCli.Commands.Reporting.AverageSpendingCreepByYear;

public class AverageSpendingCreepByYearCommandGenerator : ICommandGenerator, ITypedCommandGenerator<AverageSpendingCreepByYearCommand>
{
    public ICommand Generate(string? subCommandName, List<InstructionArgument> arguments)
    {
        return new AverageSpendingCreepByYearCommand();
    }
}