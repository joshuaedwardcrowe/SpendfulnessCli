using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Generators;
using Cli.Commands.Abstractions.Properties;
using Cli.Instructions.Abstractions;

namespace SpendfulnessCli.Commands.Reporting.AverageYearlySpending;

public class AverageYearlySpendingCliCommandGenerator : ICliCommandGenerator<AverageYearlySpendingCliCommand>
{
    public CliCommand Generate(CliInstruction instruction, List<CliCommandProperty> properties)
        => new AverageYearlySpendingCliCommand();
}