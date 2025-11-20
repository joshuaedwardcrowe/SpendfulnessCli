using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Generators;
using Cli.Instructions.Abstractions;

namespace SpendfulnessCli.Commands.Reporting.YearlySpending;

public class YearlySpendingCliCommandGenerator : ICliCommandGenerator<YearlySpendingCliCommand>
{ 
    public CliCommand Generate(CliInstruction instruction) => new YearlySpendingCliCommand();
}