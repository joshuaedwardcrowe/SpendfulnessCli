using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Factories;
using Cli.Commands.Abstractions.Properties;
using Cli.Instructions.Abstractions;

namespace SpendfulnessCli.Commands.Reporting.YearlySpending;

public class YearlySpendingCliCommandFactory : ICliCommandFactory<YearlySpendingCliCommand>
{
    public CliCommand Create(CliInstruction instruction, List<CliCommandProperty> properties)
        => new YearlySpendingCliCommand();
}