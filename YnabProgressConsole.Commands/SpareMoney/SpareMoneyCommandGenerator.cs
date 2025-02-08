using YnabProgressConsole.Instructions.InstructionArguments;

namespace YnabProgressConsole.Commands.SpareMoney;

public class SpareMoneyCommandGenerator : ICommandGenerator
{
    public ICommand Generate(List<InstructionArgument> arguments)
    {
        return new SpareMoneyCommand();
    }
}