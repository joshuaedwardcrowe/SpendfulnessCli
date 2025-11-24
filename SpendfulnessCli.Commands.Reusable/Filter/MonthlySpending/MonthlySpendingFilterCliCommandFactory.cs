using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Artefacts;
using Cli.Commands.Abstractions.Factories;
using Cli.Commands.Abstractions.Outcomes.Reusable;
using Cli.Instructions.Abstractions;
using Cli.Instructions.Arguments;
using Cli.Workflow.Commands.MissingOutcomes;
using SpendfulnessCli.Aggregation.Aggregates;
using SpendfulnessCli.Commands.Reusable.MonthlySpending;

namespace SpendfulnessCli.Commands.Reusable.Filter.MonthlySpending;

public class MonthlySpendingFilterCliCommandFactory
    : ReusableMonthlySpendingCliCommandFactory, ICliCommandFactory<FilterCliCommand>
{
    public CliCommand Create(CliInstruction instruction, List<CliCommandArtefact> artefacts)
    {
        var aggregatorArtefact = artefacts
            .OfListAggregatorType<TransactionMonthTotalAggregate>();

        if (aggregatorArtefact == null)
        {
            return new MissingOutcomesCliCommand([
                nameof(ListAggregatorCliCommandOutcome<TransactionMonthTotalAggregate>)
            ]);
        }
        
        var filterOnArgument = instruction
            .Arguments
            .OfRequiredType<string>(FilterCliCommand.ArgumentNames.FilterOn);
        
        var greaterThanArgument = instruction
            .Arguments
            .OfType<decimal>(MonthlySpendingFilterCliCommand.ArgumentNames.GreaterThan);

        return new MonthlySpendingFilterCliCommand(
            aggregatorArtefact.ArtefactValue,
            filterOnArgument.ArgumentValue,
            greaterThanArgument?.ArgumentValue);
    }
}