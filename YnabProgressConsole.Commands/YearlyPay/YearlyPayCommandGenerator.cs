using YnabProgressConsole.Instructions.InstructionArguments;

namespace YnabProgressConsole.Commands.YearlyPay;

public class YearlyPayCommandGenerator : ICommandGenerator, ITypedCommandGenerator<YearlyPayCommand>
{
    public ICommand Generate(List<InstructionArgument> arguments)
    {
        return new YearlyPayCommand();
    }
}