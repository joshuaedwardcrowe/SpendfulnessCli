using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Artefacts;
using Cli.Commands.Abstractions.Factories;
using Cli.Commands.Abstractions.Outcomes.Reusable;
using Cli.Instructions.Abstractions;
using Cli.Workflow.Commands.MissingOutcomes;
using Spendfulness.Aggregation.Aggregates;
using SpendfulnessCli.Commands.Reusable.MonthlySpending;

namespace SpendfulnessCli.Commands.Reusable.Table.MonthlySpending;

public class MonthlySpendingTableCliCommandFactory
    : MonthlySpendingCliCommandFactory, ICliCommandFactory<TableCliCommand>
{
    public CliCommand Create(CliInstruction instruction, List<CliCommandArtefact> artefacts)
    {
        var aggregatorArtefact = artefacts.OfListAggregatorType<TransactionMonthTotalAggregate>();
        
        if (aggregatorArtefact == null)
        {
            return new MissingOutcomesCliCommand([
                nameof(ListAggregatorCliCommandOutcome<TransactionMonthTotalAggregate>)
            ]);
        }

        return new MonthlySpendingTableCliCommand(aggregatorArtefact.ArtefactValue);
    }
}