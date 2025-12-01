using Cli.Commands.Abstractions.Artefacts;
using Cli.Instructions.Abstractions;
using Cli.Instructions.Arguments;
using SpendfulnessCli.Commands.Reusable.Transactions.List;

namespace SpendfulnessCli.Commands.Reusable.Filter.Transactions;

public abstract class FilterTransactionsOnCliCommandFactory : TransactionsCliCommandFactory
{
    public override bool CanCreateWhen(CliInstruction instruction, List<CliCommandArtefact> artefacts)
    {
        var previouslyCalledTransactionsCommand = base.CanCreateWhen(instruction, artefacts);

        var filterOnArgument = instruction
            .Arguments
            .OfType<string>(FilterCliCommand.ArgumentNames.FilterOn);
        
        var isValidTransactionFilter = FilterTransactionsCliCommand
            .FilterNames
            .All
            .Contains(filterOnArgument?.ArgumentValue);

        return previouslyCalledTransactionsCommand && isValidTransactionFilter;
    }
}