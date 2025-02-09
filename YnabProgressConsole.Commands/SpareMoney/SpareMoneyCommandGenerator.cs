using YnabProgressConsole.Instructions.InstructionArguments;

namespace YnabProgressConsole.Commands.SpareMoney;

public class SpareMoneyCommandGenerator : ICommandGenerator, ITypedCommandGenerator<SpareMoneyCommand>
{
    public ICommand Generate(List<InstructionArgument> arguments)
    {
        return new SpareMoneyCommand();
    }
}