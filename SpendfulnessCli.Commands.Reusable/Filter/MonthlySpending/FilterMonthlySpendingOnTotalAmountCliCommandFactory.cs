using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Artefacts;
using Cli.Commands.Abstractions.Factories;
using Cli.Instructions.Abstractions;
using Cli.Instructions.Arguments;
using SpendfulnessCli.Aggregation.Aggregates;

namespace SpendfulnessCli.Commands.Reusable.Filter.MonthlySpending;

public class FilterMonthlySpendingOnTotalAmountCliCommandFactory
    : ReusableFilterMonthlySpendingOnCliCommandFactory, ICliCommandFactory<FilterCliCommand>
{
    public override bool CanCreateWhen(CliInstruction instruction, List<CliCommandArtefact> artefacts)
    {
        var previouslyCalledMonthlySpendingCommandAndFilterArgumentPresent = base.CanCreateWhen(instruction, artefacts);

        var greaterThanArgument = instruction
            .Arguments
            .OfType<decimal>(FilterMonthlySpendingOnTotalAmountCliCommand.ArgumentNames.GreaterThan);

        return previouslyCalledMonthlySpendingCommandAndFilterArgumentPresent && greaterThanArgument != null;
    }
    
    public CliCommand Create(CliInstruction instruction, List<CliCommandArtefact> artefacts)
    {
        var aggregatorArtefact = artefacts
            .OfListAggregatorType<TransactionMonthTotalAggregate>();
        
        var filterOnArgument = instruction
            .Arguments
            .OfRequiredType<string>(FilterCliCommand.ArgumentNames.FilterOn);

        var greaterThanArgument = instruction
            .Arguments
            .OfRequiredType<decimal>(FilterMonthlySpendingOnTotalAmountCliCommand.ArgumentNames.GreaterThan);

        return new FilterMonthlySpendingOnTotalAmountCliCommand(
            aggregatorArtefact!.ArtefactValue,
            filterOnArgument.ArgumentValue,
            greaterThanArgument.ArgumentValue);
    }
}