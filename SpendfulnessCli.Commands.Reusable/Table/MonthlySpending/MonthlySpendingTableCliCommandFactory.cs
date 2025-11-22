using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Artefacts;
using Cli.Commands.Abstractions.Attributes;
using Cli.Commands.Abstractions.Factories;
using Cli.Commands.Abstractions.Outcomes.Reusable;
using Cli.Instructions.Abstractions;
using Cli.Workflow.Commands.MissingOutcomes;
using SpendfulnessCli.Aggregation.Aggregates;
using SpendfulnessCli.Commands.Reporting.MonthlySpending;

namespace SpendfulnessCli.Commands.Reusable.Table.MonthlySpending;

[FactoryFor(typeof(TableCliCommand))]
public class MonthlySpendingTableCliCommandFactory : ICliCommandFactory<MonthlySpendingTableCliCommand>
{
    public bool CanCreateWhen(CliInstruction instruction, List<CliCommandArtefact> properties)
        => properties.LastCommandRanWas<MonthlySpendingCliCommand>();
    
    public CliCommand Create(CliInstruction instruction, List<CliCommandArtefact> properties)
    {
        var monthlySpendingAggregator = properties.GetListAggregator<TransactionMonthTotalAggregate>();
        if (monthlySpendingAggregator == null)
        {
            return new MissingOutcomesCliCommand([
                nameof(CliCommandListAggregatorOutcome<TransactionMonthTotalAggregate>)
            ]);
        }
        
        return new MonthlySpendingTableCliCommand(monthlySpendingAggregator);
    }
}