using Cli.Commands.Abstractions;
using Cli.Instructions.Abstractions;

namespace SpendfulnessCli.Commands.Reporting.AverageYearlySpending;

public class AverageYearlySpendingCliCommandGenerator : ICliCommandGenerator<AverageYearlySpendingCliCommand>
{
    public CliCommand Generate(CliInstruction instruction)
    {
        return new AverageYearlySpendingCliCommand();
    }
}