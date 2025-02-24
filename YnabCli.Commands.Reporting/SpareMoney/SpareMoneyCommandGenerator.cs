using YnabCli.Instructions.Arguments;

namespace YnabCli.Commands.Reporting.SpareMoney;

public class SpareMoneyCommandGenerator : ICommandGenerator, ITypedCommandGenerator<SpareMoneyCommand>
{
    public ICommand Generate(string? subCommandName, List<InstructionArgument> arguments)
    {
        if (subCommandName == SpareMoneyCommand.SubCommandNames.Help)
        {
            return new SpareMoneyHelpCommand();
        }
        
        return GenerateDefaultCommand(arguments);
    }

    private static SpareMoneyCommand GenerateDefaultCommand(List<InstructionArgument> arguments)
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