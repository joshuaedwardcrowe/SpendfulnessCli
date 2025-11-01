using Cli.Commands.Abstractions;
using Cli.Instructions.Abstractions;
using Cli.Instructions.Arguments;
using SpendfulnessCli.Commands.Reporting.SpareMoney.Help;

namespace SpendfulnessCli.Commands.Reporting.SpareMoney;

public class SpareMoneyGenericCliCommandGenerator : ICliCommandGenerator<SpareMoneyCliCommand>
{
    public CliCommand Generate(CliInstruction instruction) =>
        instruction.SubInstructionName switch
        {
            SpareMoneyCliCommand.SubCommandNames.Help => new SpareMoneyHelpCliCommand(),
            _ => GenerateDefaultCommand(instruction.Arguments)
        };

    private static SpareMoneyCliCommand GenerateDefaultCommand(List<CliInstructionArgument> arguments)
    {
        var addArgument = arguments.OfCurrencyType(SpareMoneyCliCommand.ArgumentNames.Add);
        var minusArgument = arguments.OfCurrencyType(SpareMoneyCliCommand.ArgumentNames.Minus);
        var minusSavingsArgument = arguments.OfType<bool>(SpareMoneyCliCommand.ArgumentNames.MinusSavings);

        return new SpareMoneyCliCommand
        {
            Add = addArgument?.ArgumentValue,
            Minus = minusArgument?.ArgumentValue,
            MinusSavings = minusSavingsArgument?.ArgumentValue
        };
    }
}