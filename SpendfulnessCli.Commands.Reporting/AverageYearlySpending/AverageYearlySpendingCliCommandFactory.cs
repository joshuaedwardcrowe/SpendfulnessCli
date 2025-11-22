using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Factories;
using Cli.Commands.Abstractions.Properties;
using Cli.Instructions.Abstractions;

namespace SpendfulnessCli.Commands.Reporting.AverageYearlySpending;

public class AverageYearlySpendingCliCommandFactory : ICliCommandFactory<AverageYearlySpendingCliCommand>
{
    public CliCommand Create(CliInstruction instruction, List<CliCommandProperty> properties)
        => new AverageYearlySpendingCliCommand();
}