using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Artefacts;
using Cli.Commands.Abstractions.Factories;
using Cli.Instructions.Abstractions;
using Cli.Instructions.Arguments;
using Ynab;

namespace SpendfulnessCli.Commands.Reusable.Filter.Transactions.PayeeName.Equals;

public class FilterTransactionsOnPayeeNameEqualsCliCommandFactory 
    : FilterTransactionsOnCliCommandFactory, ICliCommandFactory<FilterCliCommand>
{
    public override bool CanCreateWhen(CliInstruction instruction, List<CliCommandArtefact> artefacts)
    {
        var previousCalledTransactionsCommandAndFilterArgumentPresent = base.CanCreateWhen(instruction, artefacts);
        
        var payeeNameArgument = instruction
            .Arguments
            .OfType<string>(FilterTransactionsOnPayeeNameEqualsCliCommand.ArgumentNames.Is);
        
        return previousCalledTransactionsCommandAndFilterArgumentPresent && payeeNameArgument != null;
    }

    public CliCommand Create(CliInstruction instruction, List<CliCommandArtefact> artefacts)
    {
        var aggregatorArtefact = artefacts
            .OfListAggregatorType<Transaction>();
        
        var filterOnArgument = instruction
            .Arguments
            .OfRequiredType<string>(FilterCliCommand.ArgumentNames.FilterOn);

        var payeeNameArgument = instruction
            .Arguments
            .OfRequiredType<string>(FilterTransactionsOnPayeeNameEqualsCliCommand.ArgumentNames.Is);

        return new FilterTransactionsOnPayeeNameEqualsCliCommand(
            aggregatorArtefact!.ArtefactValue,
            filterOnArgument.ArgumentValue,
            payeeNameArgument.ArgumentValue);
    }
}