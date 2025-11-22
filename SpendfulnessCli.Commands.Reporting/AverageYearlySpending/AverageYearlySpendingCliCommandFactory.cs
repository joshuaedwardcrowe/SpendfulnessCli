using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Artefacts;
using Cli.Commands.Abstractions.Factories;
using Cli.Instructions.Abstractions;

namespace SpendfulnessCli.Commands.Reporting.AverageYearlySpending;

public class AverageYearlySpendingCliCommandFactory : ICliCommandFactory<AverageYearlySpendingCliCommand>
{
    public CliCommand Create(CliInstruction instruction, List<CliCommandArtefact> properties)
        => new AverageYearlySpendingCliCommand();
}