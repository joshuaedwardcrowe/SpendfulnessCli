using YnabCli.Instructions.Arguments;

namespace YnabCli.Commands.Reporting.YearlyPay;

public class YearlyPayCommandGenerator : ICommandGenerator, ITypedCommandGenerator<YearlyPayCommand>
{
    public ICommand Generate(string? subCommandName, List<InstructionArgument>? arguments)
    {
        return new YearlyPayCommand();
    }
}