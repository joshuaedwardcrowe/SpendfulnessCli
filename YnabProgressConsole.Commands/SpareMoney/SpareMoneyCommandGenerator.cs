using YnabProgressConsole.Instructions.InstructionArguments;

namespace YnabProgressConsole.Commands.SpareMoney;

public class SpareMoneyCommandGenerator : ICommandGenerator, ITypedCommandGenerator<SpareMoneyCommand>
{
    public ICommand Generate(List<InstructionArgument> arguments)
    {
        var minusArgument = GetMinusArgument(arguments);
        
        var minusSavingsArgument = arguments.OfType<bool>(SpareMoneyCommand.ArgumentNames.MinusSavings);
        
        return new SpareMoneyCommand
        {
            Minus = minusArgument?.ArgumentValue,
            MinusSavings = minusSavingsArgument?.ArgumentValue
        };
    }

    private static TypedInstructionArgument<decimal>? GetMinusArgument(List<InstructionArgument> arguments)
    {
        var minusIntArgument = arguments.OfType<int>(SpareMoneyCommand.ArgumentNames.Minus);
        if (minusIntArgument != null)
        {
            decimal conversionToDecimal = minusIntArgument.ArgumentValue;
            return new TypedInstructionArgument<decimal>(minusIntArgument.ArgumentName, conversionToDecimal);
        }
        
        return arguments.OfType<decimal>(SpareMoneyCommand.ArgumentNames.Minus);
    }
}