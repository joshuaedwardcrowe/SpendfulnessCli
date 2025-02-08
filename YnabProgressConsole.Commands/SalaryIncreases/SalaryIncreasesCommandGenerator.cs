using YnabProgressConsole.Instructions.InstructionArguments;

namespace YnabProgressConsole.Commands.SalaryIncreases;

public class SalaryIncreasesCommandGenerator : ICommandGenerator
{
    public ICommand Generate(List<InstructionArgument> arguments)
    {
        return new SalaryIncreasesCommand();
    }
}