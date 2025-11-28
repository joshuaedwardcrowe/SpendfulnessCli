using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Artefacts;
using Cli.Commands.Abstractions.Factories;
using Cli.Instructions.Abstractions;

namespace SpendfulnessCli.Commands.Reporting.SplittableTransactions;

public class SplittableTransactionsCliCommandFactory : ICliCommandFactory<SplittableTransactionsCliCommand>
{
    public CliCommand Create(CliInstruction instruction, List<CliCommandArtefact> artefacts)
        => new SplittableTransactionsCliCommand();
}