using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Attributes;
using Cli.Commands.Abstractions.Generators;
using Cli.Commands.Abstractions.Properties;
using Cli.Instructions.Abstractions;
using SpendfulnessCli.Aggregation.Aggregates;

namespace SpendfulnessCli.Commands.Reusable.Table.MonthlySpending;

[CliCommandGeneratorFor(typeof(TableCliCommand))]
public class MonthlySpendingTableCliCommandGenerator : ICliCommandGenerator<MonthlySpendingTableCliCommand>
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