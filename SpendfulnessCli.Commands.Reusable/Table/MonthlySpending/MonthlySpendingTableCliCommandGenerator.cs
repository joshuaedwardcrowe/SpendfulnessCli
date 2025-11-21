using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Attributes;
using Cli.Commands.Abstractions.Factories;
using Cli.Commands.Abstractions.Properties;
using Cli.Instructions.Abstractions;
using SpendfulnessCli.Aggregation.Aggregates;

namespace SpendfulnessCli.Commands.Reusable.Table.MonthlySpending;

[FactoryFor(typeof(TableCliCommand))]
public class MonthlySpendingTableCliCommandGenerator : ICliCommandFactory<MonthlySpendingTableCliCommand>
{
    public bool CanGenerate(CliInstruction instruction, List<CliCommandProperty> properties)
    {
        return properties
            .OfType<ListAggregatorCliCommandProperty<TransactionMonthTotalAggregate>>()
            .Any();
    }
    
    public CliCommand Generate(CliInstruction instruction, List<CliCommandProperty> properties)
    {
        var monthlySpendingAggregatorProperty = properties
            .OfType<ListAggregatorCliCommandProperty<TransactionMonthTotalAggregate>>()
            .First();

        return new MonthlySpendingTableCliCommand(monthlySpendingAggregatorProperty.Value);
    }
}