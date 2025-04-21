using YnabCli.Commands.Generators;
using YnabCli.Instructions.Arguments;

namespace YnabCli.Commands.Reporting.AverageYearlyPay;

public class AverageYearlyPayCommandGenerator : ICommandGenerator, ITypedCommandGenerator<AverageYearlyPayCommand>
{
    public ICommand Generate(string? subCommandName, List<InstructionArgument>? arguments)
    {
        return new AverageYearlyPayCommand();
    }
}