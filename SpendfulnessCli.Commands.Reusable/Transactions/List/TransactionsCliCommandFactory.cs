using Cli.Commands.Abstractions.Artefacts;
using Cli.Instructions.Abstractions;
using SpendfulnessCli.Commands.Reporting.Transactions.List;

namespace SpendfulnessCli.Commands.Reusable.Transactions.List;

public abstract class TransactionsCliCommandFactory
{
    public virtual bool CanCreateWhen(CliInstruction instruction, List<CliCommandArtefact> artefacts)
    {
        var ranAggregatorCommand = artefacts.LastCommandRanWas<ListTransactionCliCommand>();

        return ranAggregatorCommand;
    }
}