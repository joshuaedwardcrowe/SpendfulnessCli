using Cli.Commands.Abstractions;
using Cli.Instructions.Abstractions;

namespace Cli.Ynab.Commands.Reporting.AverageYearlyPay;

public class AverageYearlyPayGenericCommandGenerator : ICommandGenerator<AverageYearlyPayCommand>
{
    public ICommand Generate(string? subCommandName, List<ConsoleInstructionArgument>? arguments)
    {
        return new AverageYearlyPayCommand();
    }
}