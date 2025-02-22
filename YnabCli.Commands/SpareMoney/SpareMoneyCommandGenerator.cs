using YnabCli.Instructions.InstructionArguments;

namespace YnabCli.Commands.SpareMoney;

public class SpareMoneyCommandGenerator : ICommandGenerator, ITypedCommandGenerator<SpareMoneyCommand>
{
    public ICommand Generate(List<InstructionArgument> arguments)
    {
        var minusArgument = arguments.OfCurrencyType(SpareMoneyCommand.ArgumentNames.Minus);
        var minusSavingsArgument = arguments.OfType<bool>(SpareMoneyCommand.ArgumentNames.MinusSavings);

        return new SpareMoneyCommand
        {
            Minus = minusArgument?.ArgumentValue,
            MinusSavings = minusSavingsArgument?.ArgumentValue
        };
    }
}