using KitCli.Commands.Abstractions;
using KitCli.Commands.Abstractions.Artefacts;
using KitCli.Commands.Abstractions.Factories;
using KitCli.Instructions.Abstractions;
using KitCli.Instructions.Arguments;
using YnabSharp;

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