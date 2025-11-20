using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Generators;
using Cli.Commands.Abstractions.Properties;
using Cli.Instructions.Abstractions;
using SpendfulnessCli.Aggregation.Aggregates;
using SpendfulnessCli.Commands.Reusable.Table.MonthlySpending;

namespace SpendfulnessCli.Commands.Reusable.Table;

public class TableCliCommandGenerator : ICliCommandGenerator<TableCliCommand>
{
    public CliCommand Generate(CliInstruction instruction, List<CliCommandProperty> properties)
    {
        var monthlySpendingAggregatorProperty = properties
            .OfType<ListAggregatorCliCommandProperty<TransactionMonthTotalAggregate>>()
            .FirstOrDefault();

        if (monthlySpendingAggregatorProperty != null)
        {
            return new MonthlySpendingTableCliCommand(monthlySpendingAggregatorProperty.Value);
        }
        
        return new TableCliCommand();
    }
}