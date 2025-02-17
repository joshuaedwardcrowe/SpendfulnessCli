using YnabProgressConsole.Instructions.InstructionArguments;

namespace YnabProgressConsole.Commands.SpareMoney;

public class SpareMoneyCommandGenerator : ICommandGenerator, ITypedCommandGenerator<SpareMoneyCommand>
{
    public ICommand Generate(List<InstructionArgument> arguments)
    {
        var minusSavingsArgument = arguments.OfType<bool>(SpareMoneyCommand.ArgumentNames.MinusSavings);

        return new SpareMoneyCommand
        {
            MinusSavings = minusSavingsArgument?.ArgumentValue
        };
    }
}