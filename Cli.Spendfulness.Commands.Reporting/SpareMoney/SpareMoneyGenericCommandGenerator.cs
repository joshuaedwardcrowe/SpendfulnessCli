using Cli.Commands.Abstractions;
using Cli.Instructions.Abstractions;
using Cli.Instructions.Arguments;
using Cli.Ynab.Commands.Reporting.SpareMoney.Help;

namespace Cli.Ynab.Commands.Reporting.SpareMoney;

public class SpareMoneyGenericCommandGenerator : ICommandGenerator<SpareMoneyCommand>
{
    public ICommand Generate(string? subCommandName, List<ConsoleInstructionArgument> arguments)
    {
        if (subCommandName == SpareMoneyCommand.SubCommandNames.Help)
        {
            return new SpareMoneyHelpCommand();
        }
        
        return GenerateDefaultCommand(arguments);
    }

    private static SpareMoneyCommand GenerateDefaultCommand(List<ConsoleInstructionArgument> arguments)
    {
        var addArgument = arguments.OfCurrencyType(SpareMoneyCommand.ArgumentNames.Add);
        var minusArgument = arguments.OfCurrencyType(SpareMoneyCommand.ArgumentNames.Minus);
        var minusSavingsArgument = arguments.OfType<bool>(SpareMoneyCommand.ArgumentNames.MinusSavings);

        return new SpareMoneyCommand
        {
            Add = addArgument?.ArgumentValue,
            Minus = minusArgument?.ArgumentValue,
            MinusSavings = minusSavingsArgument?.ArgumentValue
        };
    }
}